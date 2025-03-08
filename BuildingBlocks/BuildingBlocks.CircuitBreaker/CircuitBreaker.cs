namespace MilestoneDeviceAdapter.VideoSequenceExtraction.CircuitBreaker;

public class CircuitBreaker(string resource, int openToHalfOpenWaitTimeInMinutes)
{
    private static readonly string SequenceExtractorCircuitBreakerFailureThreshold = Environment.GetEnvironmentVariable("SEQUENCE_EXTRACTOR_CIRCUIT_BREAKER_FAILURE_THRESHOLD") ?? "5";

    private readonly ICircuitBreakerStateStore stateStore = CircuitBreakerStateStoreFactory.GetCircuitBreakerStateStore(resource);

    private readonly object halfOpenSyncObject = new();

    private readonly string resourceName = resource;

    private readonly TimeSpan openToHalfOpenWaitTime = new(0, 0, openToHalfOpenWaitTimeInMinutes, 0, 0);

    public bool IsClosed { get { return stateStore.IsClosed; } }

    public bool IsOpen { get { return !IsClosed; } }

    public string ResourceName { get { return resourceName; } }

    private int failureCounter = 0;

    public async Task Invoke(Func<Task> action)
    {
        await Task.Run(async () =>
        {
            if (IsOpen)
            {
                // The circuit breaker is Open
                await WhenCircuitIsOpen(action);
                return;
            }

            // The circuit breaker is Closed, execute the action
            try
            {
                await action();
            }
            catch (Exception e)
            {
                // If an exception still occurs here, simply re-trip the breaker immediately
                TrackException(e);

                // Throw the exception so that the caller can tell the type of exception that was thrown
                throw;
            }
        });
    }

    /*public Task Invoke(Action action, Action<CircuitBreakerOpenException> circuitBreakerOpenAction, Action<Exception> anyOtherExceptionAction)
    {
        return Task.Run(() =>
        {
            try
            {
                Invoke(action);
            }
            catch (CircuitBreakerOpenException cboe)
            {
                // Perform some different action when the breaker is open. Last exception details are in the inner exception
                circuitBreakerOpenAction(cboe);
            }
            catch (Exception e)
            {
                anyOtherExceptionAction(e);
            }
        });
    }*/

    private void TrackException(Exception e)
    {
        // Open the circuit breaker If the number of recent failures exceeds a specified threshold
        if (e is MilestoneConnectionException)
        {
            if (stateStore.IsClosed)
            {
                Interlocked.Increment(ref failureCounter);
                _ = int.TryParse(SequenceExtractorCircuitBreakerFailureThreshold, out int failureThreshold);
                if (failureCounter == failureThreshold)
                {
                    stateStore.Open(e);
                }
            } 
            else if (stateStore.IsHalfOpen) 
            {
                stateStore.Open(e);
            }
        }
    }

    private async Task WhenCircuitIsOpen(Func<Task> action)
    {
        await Task.Run(async () =>
        {
            // The circuit breaker is Open. Check if the Open timeout has expired.
            // If it has, set the state to HalfOpen
            if (stateStore.LastStateChangeDateUtc + openToHalfOpenWaitTime < DateTime.UtcNow)
            {
                // The Open timeout has expired. Allow one operation to execute.
                // The circuit breaker is simply set to HalfOpen after being in the Open state for some period of time
                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(halfOpenSyncObject, ref lockTaken);
                    if (lockTaken)
                    {
                        // Set the circuit breaker state to HalfOpen
                        stateStore.HalfOpen();

                        // Attempt the operation.
                        await action();

                        // If this action succeeds, reset the state and allow other operations
                        ResetFailureCounter();
                        stateStore.Close();
                        return;
                    }
                }
                catch (Exception e)
                {
                    // If there is still an exception, trip the breaker again immediately
                    TrackException(e);

                    // Throw the exception so that the caller knows which exception occurred
                    throw new CircuitBreakerOpenException(
                        "The circuit was tripped while half-open. Refer to the inner exception for the cause of the trip.", e);
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(halfOpenSyncObject);
                    }
                }
            }

            // The Open timeout has not yet expired. Throw a CircuitBreakerOpen exception to
            // inform the caller that the caller that the call was not actually attempted, 
            // and return the most recent exception received
            throw new CircuitBreakerOpenException(
                "The circuit is still open. Refer to the inner exception for the cause of the circuit trip.", stateStore.LastException);
        });
    }

    private void ResetFailureCounter()
    {
        Interlocked.Exchange(ref failureCounter, 0);
    }
}
