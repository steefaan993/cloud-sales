namespace BuildingBlocks.Messaging.Events;

public record IntegrationEvent
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public DateTime Timestamp { get; private init; } = DateTime.Now;
    public string? EventType => GetType()?.AssemblyQualifiedName;
}
