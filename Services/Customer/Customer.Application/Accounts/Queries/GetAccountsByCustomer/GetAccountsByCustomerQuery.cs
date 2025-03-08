using BuildingBlocks.Pagination;

namespace Customer.Application.Accounts.Queries.GetOrdersByCustomer;

public record GetAccountsByCustomerQuery(Guid CustomerId, PageRequest PageRequest) 
    : IQuery<GetAccountsByCustomerResult>;

public record GetAccountsByCustomerResult(Guid CustomerId, PageResult<AccountData> Accounts);
