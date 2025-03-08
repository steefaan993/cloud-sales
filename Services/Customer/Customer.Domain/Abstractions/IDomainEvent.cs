using MediatR;

namespace Customer.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    public DateTime Timestamp { get; }
}
