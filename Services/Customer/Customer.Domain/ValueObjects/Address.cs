namespace Customer.Domain.ValueObjects;

public record Address
{
    public string StreetAddress { get; } = default!;
    public string Country { get; } = default!;
    public string City { get; } = default!;
    public string PostalCode { get; } = default!;

    protected Address()
    {
    }

    private Address(string streetAddress, string country, string city, string postalCode)
    {
        StreetAddress = streetAddress;
        Country = country;
        City = city;
        PostalCode = postalCode;
    }

    public static Address Of(string streetAddress, string country, string city, string postalCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(streetAddress, "Street address can not be null or empty");
        ArgumentException.ThrowIfNullOrWhiteSpace(postalCode, "Postal code can not be null or empty");

        return new Address(streetAddress, country, city, postalCode);
    }
}
