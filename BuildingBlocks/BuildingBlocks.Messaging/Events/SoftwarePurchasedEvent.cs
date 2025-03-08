namespace BuildingBlocks.Messaging.Events;

public record SoftwarePurchasedEvent : IntegrationEvent
{
    public Guid AccountId { get; set; }
    public Guid SubscriptionId { get; set; }
    public string SoftwareName { get; set; } = default!;
    public string Vendor { get; set; } = default!;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int Quantity { get; set; }
}
