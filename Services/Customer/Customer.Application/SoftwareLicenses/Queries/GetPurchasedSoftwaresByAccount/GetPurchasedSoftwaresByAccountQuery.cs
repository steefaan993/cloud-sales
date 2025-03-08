using BuildingBlocks.Pagination;

namespace Customer.Application.SoftwareLicenses.Queries.GetOrdersByCustomer;

public record GetPurchasedSoftwaresByAccountQuery(Guid AccountId, PageRequest PageRequest) 
    : IQuery<GetPurchasedSoftwaresByAccountResult>;

public record GetPurchasedSoftwaresByAccountResult(Guid AccountId, PageResult<SoftwareLicenseData> SoftwareLicenses);
