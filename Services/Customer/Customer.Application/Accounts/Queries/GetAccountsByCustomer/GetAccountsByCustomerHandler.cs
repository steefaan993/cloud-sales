using BuildingBlocks.Pagination;
using FluentValidation;

namespace Customer.Application.Accounts.Queries.GetOrdersByCustomer;

public class GetAccountsByCustomerQueryValidator : AbstractValidator<GetAccountsByCustomerQuery>
{
    public GetAccountsByCustomerQueryValidator()
    {
        RuleFor(q => q.CustomerId).NotEmpty().WithMessage("Customer ID is required");
        RuleFor(q => q.PageRequest.Page).GreaterThanOrEqualTo(0).WithMessage("Page index must be greater than or equal to 0");
        RuleFor(q => q.PageRequest.Size).GreaterThan(0).WithMessage("Page size must be greater than 0");
    }
}

public class GetAccountsByCustomerHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetAccountsByCustomerQuery, GetAccountsByCustomerResult>
{
    public async Task<GetAccountsByCustomerResult> Handle(GetAccountsByCustomerQuery query, CancellationToken cancellationToken)
    {
        if (!await dbContext.Customers.AnyAsync(c => c.Id == CustomerId.Of(query.CustomerId), cancellationToken: cancellationToken))
        {
            throw new CustomerNotFoundException(query.CustomerId);
        }

        var pageIndex = query.PageRequest.Page;
        var pageSize = query.PageRequest.Size;

        var totalCount = await dbContext.Accounts.Where(a => a.CustomerId == CustomerId.Of(query.CustomerId)).LongCountAsync(cancellationToken);

        var accounts = await dbContext.Accounts
                        .AsNoTracking()
                        .Where(a => a.CustomerId == CustomerId.Of(query.CustomerId))
                        .OrderBy(a => a.Name)
                        .ThenBy(a => a.Department)
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .Select(a => a.ToAccountData())
                        .ToListAsync(cancellationToken);

        return new GetAccountsByCustomerResult(
            query.CustomerId,
            new PageResult<AccountData>(
                pageIndex,
                pageSize,
                totalCount,
                accounts));    
    }
}
