namespace MilestoneDeviceAdapter.VideoSequenceExtraction.CircuitBreaker
{
    public interface ICircuitBreakerStateStore
    {
        string Name { get; } // set by a factory
        CircuitBreakerState State { get; } // closed = green light, open = red light, half-open = yellow light
        Exception? LastException { get; } // set when the stateStore trips (breaker state changes)
        DateTime? LastStateChangeDateUtc { get; } // same as LastException
        void Open(Exception e); // executed when number of recent failures exceeds a specified threshold
        void Close(); // executed after a half-open breaker doesn't hit more exceptions
        void HalfOpen(); // executed after a breaker has been open for 30s w/o exceptions
        bool IsClosed { get; } // check the stateStore to see if the break is closed
        bool IsHalfOpen { get; } // check the stateStore to see if the break is half open
    }
}
