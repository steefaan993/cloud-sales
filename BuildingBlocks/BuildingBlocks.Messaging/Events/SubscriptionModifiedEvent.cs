namespace BuildingBlocks.Messaging.Events;

public record SubscriptionModifiedEvent : IntegrationEvent
{
    public Guid SubscriptionId { get; set; }
    public int Quantity { get; set; }
}
