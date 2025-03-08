namespace Customer.Domain.ValueObjects;

public record CustomerId
{
    public Guid Value { get; }
    private CustomerId(Guid value) => Value = value;
    
    public static CustomerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value, "Customer ID can not be null");
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Customer ID cannot be empty");
        }

        return new CustomerId(value);
    }
}
