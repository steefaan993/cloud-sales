namespace MilestoneDeviceAdapter.VideoSequenceExtraction.CircuitBreaker;

public enum CircuitBreakerState
{
    None = 1,
    Open = 2,
    Closed = 3,
    HalfOpen = 4
}
