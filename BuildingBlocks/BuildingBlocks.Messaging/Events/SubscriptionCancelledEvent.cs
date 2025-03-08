namespace BuildingBlocks.Messaging.Events;

public record SubscriptionCancelledEvent : IntegrationEvent
{
    public Guid SubscriptionId { get; set; }
}
