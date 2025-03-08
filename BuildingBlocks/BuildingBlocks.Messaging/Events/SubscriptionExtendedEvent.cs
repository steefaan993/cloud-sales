namespace BuildingBlocks.Messaging.Events;

public record SubscriptionExtendedEvent : IntegrationEvent
{
    public Guid SubscriptionId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}
