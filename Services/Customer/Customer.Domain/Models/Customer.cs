namespace Customer.Domain.Models;

public class Customer : Aggregate<CustomerId>
{
    public string Name { get; set; } = default!;
    public string CompanyRegistrationNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public CustomerId? ResellerId { get; set; }
    public Customer? Reseller { get; set; }
    public List<Account> Accounts { get; private set; } = [];

    public static Customer Create(CustomerId id, string name, string companyRegistrationNumber, string email, string phoneNumber, Address address, CustomerId? ResellerId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, "Name can not be null or empty");
        ArgumentException.ThrowIfNullOrWhiteSpace(companyRegistrationNumber, "CRN can not be null or empty");
        ArgumentException.ThrowIfNullOrWhiteSpace(email, "Email can not be null or empty");
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber, "Phone number can not be null or empty");

        var customer = new Customer
        {
            Id = id,
            Name = name,
            CompanyRegistrationNumber = companyRegistrationNumber,
            Email = email,
            PhoneNumber = phoneNumber,
            Address = address,
            ResellerId = ResellerId
        };

        return customer;
    }
}
