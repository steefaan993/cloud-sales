namespace Customer.Domain.Models;

public class Reseller : Customer
{
    public decimal? CommissionPercentage { get; set; }
    public decimal? BaseDiscountRate { get; set; }

    public static Reseller Create(CustomerId id, string name, string companyRegistrationNumber, string email, string phoneNumber, Address address,
        decimal? commissionPercentage = null, decimal? baseDiscountRate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, "Name can not be null or empty");
        ArgumentException.ThrowIfNullOrWhiteSpace(companyRegistrationNumber, "CRN can not be null or empty");
        ArgumentException.ThrowIfNullOrWhiteSpace(email, "Email can not be null or empty");
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber, "Phone number can not be null or empty");

        var reseller = new Reseller
        {
            Id = id,
            Name = name,
            CompanyRegistrationNumber = companyRegistrationNumber,
            Email = email,
            PhoneNumber = phoneNumber,
            Address = address,
            CommissionPercentage = commissionPercentage,
            BaseDiscountRate = baseDiscountRate
        };

        return reseller;
    }
}
