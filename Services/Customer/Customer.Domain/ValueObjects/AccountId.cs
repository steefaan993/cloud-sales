namespace Customer.Domain.ValueObjects;

public record AccountId
{
    public Guid Value { get; }
    private AccountId(Guid value) => Value = value;
    
    public static AccountId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value, "Account ID cannot be null");
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Account ID cannot be empty");
        }

        return new AccountId(value);
    }
}
