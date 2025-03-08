namespace Customer.Domain.Models;

public class Account : Aggregate<AccountId>
{
    public CustomerId CustomerId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Department { get; set; } = default!;
    public List<SoftwareLicense> SoftwareLicenses { get; private set; } = [];

    public static Account Create(AccountId id, CustomerId customerId, string name, string department)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, "Name can not be null or empty");
        ArgumentException.ThrowIfNullOrWhiteSpace(department, "Department can not be null or empty");

        var account = new Account
        {
            Id = id,
            CustomerId = customerId,
            Name = name,
            Department = department
        };

        return account;
    }

    public void AddSoftwareLicense(string vendor, string softwareName, int quantity, Guid referenceId, DateTime validFrom, DateTime validTo)
    {
        SoftwareLicenses.Add(SoftwareLicense.Create(SoftwareLicenseId.Of(Guid.NewGuid()), Id, vendor, softwareName, quantity, referenceId, validFrom, validTo));
    }
}
