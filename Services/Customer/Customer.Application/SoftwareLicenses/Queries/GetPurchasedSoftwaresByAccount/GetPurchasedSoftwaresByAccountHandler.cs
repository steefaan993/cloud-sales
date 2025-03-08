using BuildingBlocks.Pagination;
using FluentValidation;

namespace Customer.Application.SoftwareLicenses.Queries.GetOrdersByCustomer;

public class GetPurchasedSoftwaresByAccountQueryValidator : AbstractValidator<GetPurchasedSoftwaresByAccountQuery>
{
    public GetPurchasedSoftwaresByAccountQueryValidator()
    {
        RuleFor(q => q.AccountId).NotEmpty().WithMessage("Account ID is required");
        RuleFor(q => q.PageRequest.Page).GreaterThanOrEqualTo(0).WithMessage("Page index must be greater than or equal to 0");
        RuleFor(q => q.PageRequest.Size).GreaterThan(0).WithMessage("Page size must be greater than 0");
    }
}

internal class GetPurchasedSoftwaresByAccountHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetPurchasedSoftwaresByAccountQuery, GetPurchasedSoftwaresByAccountResult>
{
    public async Task<GetPurchasedSoftwaresByAccountResult> Handle(GetPurchasedSoftwaresByAccountQuery query, CancellationToken cancellationToken)
    {
        if (!await dbContext.Accounts.AnyAsync(a => a.Id == AccountId.Of(query.AccountId), cancellationToken: cancellationToken))
        {
            throw new AccountNotFoundException(query.AccountId);
        }

        var pageIndex = query.PageRequest.Page;
        var pageSize = query.PageRequest.Size;

        var totalCount = await dbContext.SoftwareLicenses.Where(sl => sl.AccountId == AccountId.Of(query.AccountId)).LongCountAsync(cancellationToken);

        var softwareLicenses = await dbContext.SoftwareLicenses
                        .AsNoTracking()
                        .Where(sl => sl.AccountId == AccountId.Of(query.AccountId))
                        .OrderBy(sl => sl.Vendor)
                        .ThenBy(sl => sl.SoftwareName)
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .Select(sl => sl.ToSoftwareLicenseData())
                        .ToListAsync(cancellationToken);

        return new GetPurchasedSoftwaresByAccountResult(
            query.AccountId,
            new PageResult<SoftwareLicenseData>(
                pageIndex,
                pageSize,
                totalCount,
                softwareLicenses));
    }
}
