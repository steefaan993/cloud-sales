namespace Customer.Application.Dtos;

public record CustomerData(
    string Name,
    string CompanyRegistrationNumber,
    string Email,
    string PhoneNumber,
    AddressData Address,
    string Type);
