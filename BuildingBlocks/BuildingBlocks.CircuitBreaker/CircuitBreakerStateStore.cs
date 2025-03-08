using System.Collections.Concurrent;

namespace MilestoneDeviceAdapter.VideoSequenceExtraction.CircuitBreaker;

public class CircuitBreakerStateStore(string key) : ICircuitBreakerStateStore
{
    private readonly ConcurrentStack<Exception?> exceptionsSinceLastStateChange = new();

    public string Name { get; private set; } = key;

    private CircuitBreakerState state;
    public CircuitBreakerState State
    {
        get
        {
            if (state.Equals(CircuitBreakerState.None))
            {
                state = CircuitBreakerState.Closed;
            }
            return state;
        }
        private set
        {
            state = value;
        }
    }

    public Exception? LastException
    {
        get
        {
            _ = exceptionsSinceLastStateChange.TryPeek(out Exception? lastException);
            return lastException;
        }
    }

    private DateTime? lastStateChangeDateUtc;
    public DateTime? LastStateChangeDateUtc
    {
        get
        {
            return lastStateChangeDateUtc;
        }
        private set
        {
            lastStateChangeDateUtc = value;
        }
    }

    public void Open(Exception e)
    {
        ChangeState(CircuitBreakerState.Open);
        exceptionsSinceLastStateChange.Push(e);
    }

    public void Close()
    {
        ChangeState(CircuitBreakerState.Closed);
        exceptionsSinceLastStateChange.Clear();
    }

    public void HalfOpen()
    {
        ChangeState(CircuitBreakerState.HalfOpen);
    }

    public bool IsClosed
    {
        get
        {
            return State.Equals(CircuitBreakerState.Closed);
        }
    }

    public bool IsHalfOpen
    {
        get
        {
            return State.Equals(CircuitBreakerState.HalfOpen);
        }
    }

    private void ChangeState(CircuitBreakerState state)
    {
        State = state;
        LastStateChangeDateUtc = DateTime.UtcNow;
    }
}
