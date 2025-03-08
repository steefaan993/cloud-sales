namespace Customer.Domain.Models;

public class SoftwareLicense : Entity<SoftwareLicenseId>
{
    public AccountId AccountId { get; set; } = default!;
    public string Vendor { get; set; } = default!;
    public string SoftwareName { get; set; } = default!;
    public int Quantity { get; set; }
    public SubscriptionState State { get; set; }
    public Guid ReferenceId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public static SoftwareLicense Create(SoftwareLicenseId id, AccountId accountId, string vendor, string softwareName, int quantity, Guid referenceId, DateTime validFrom, DateTime validTo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(vendor, "Vendor can not be null or empty");
        ArgumentException.ThrowIfNullOrWhiteSpace(softwareName, "Software name can not be null or empty");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, "Quantity can not be negative or zero");
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(validTo, DateTime.Now, "Valid to date can not be less than or equal to current date");

        var softwareLicense = new SoftwareLicense
        {
            Id = id,
            AccountId = accountId,
            Vendor = vendor,
            SoftwareName = softwareName,
            Quantity = quantity,
            State = SubscriptionState.Active,
            ReferenceId = referenceId,
            ValidFrom = validFrom,
            ValidTo = validTo
        };

        return softwareLicense;
    }

    public void Extend(DateTime validTo)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(validTo, DateTime.Now, "Valid to date can not be less than or equal to current date");
        ValidTo = validTo;
    }

    public void Cancel()
    {
        if (State is not (SubscriptionState.Expired or SubscriptionState.Terminated))
            State = SubscriptionState.Cancelled;
    }

    public void Modify(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, "Quantity can not be negative or zero");
        Quantity = quantity;
    }
}
