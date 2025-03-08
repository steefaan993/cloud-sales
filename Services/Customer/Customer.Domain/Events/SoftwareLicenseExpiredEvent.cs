namespace Customer.Domain.Events;

public record SoftwareLicenseExpiredEvent(SoftwareLicense SoftwareLicense) : IDomainEvent
{
    public Guid EventId { get; private init; } = Guid.NewGuid();
    public DateTime Timestamp { get; private init; } = DateTime.Now;
}
