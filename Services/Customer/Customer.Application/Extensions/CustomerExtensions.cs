using Customer.Domain.Enums;

namespace Customer.Application.Extensions;

public static class CustomerExtensions
{
    public static IEnumerable<CustomerData> ToCustomerDataList(this IEnumerable<Domain.Models.Customer> customers)
    {
        return customers.Select(DataFromCustomer);
    }

    public static CustomerData ToCustomerData(this Domain.Models.Customer customer)
    {
        return DataFromCustomer(customer);
    }

    private static CustomerData DataFromCustomer(Domain.Models.Customer customer)
    {
        return new CustomerData(
                    Name: customer.Name,
                    CompanyRegistrationNumber: customer.CompanyRegistrationNumber,
                    Email: customer.Email,
                    PhoneNumber: customer.PhoneNumber,
                    Address: new AddressData(customer.Address.StreetAddress, customer.Address.Country, customer.Address.City, customer.Address.PostalCode),
                    Type: customer is Reseller ? nameof(CustomerType.Reseller) : nameof(CustomerType.EndCustomer)
                );
    }
}
