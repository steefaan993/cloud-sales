namespace Customer.Domain.ValueObjects;

public record SoftwareLicenseId
{
    public Guid Value { get; }
    private SoftwareLicenseId(Guid value) => Value = value;
    
    public static SoftwareLicenseId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value, "Software license ID can not be null");
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Software license ID cannot be empty");
        }

        return new SoftwareLicenseId(value);
    }
}
