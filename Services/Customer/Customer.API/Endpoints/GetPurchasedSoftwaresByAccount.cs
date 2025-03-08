using Customer.Application.SoftwareLicenses.Queries.GetOrdersByCustomer;
using BuildingBlocks.Pagination;

namespace Customer.API.Endpoints;

public record GetPurchasedSoftwaresByAccountRequest(GetPurchasedSoftwaresByAccountQuery GetPurchasedSoftwaresByAccountQuery);
public record GetPurchasedSoftwaresByAccountResponse(Guid AccountId, PageResult<SoftwareLicenseData> SoftwareLicenses);

public class GetPurchasedSoftwaresByAccount : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/accounts/{accountId}/purchased-softwares", async (Guid accountId, [AsParameters] PageRequest pageRequest, ISender sender) =>
        {
            var result = await sender.Send(new GetPurchasedSoftwaresByAccountQuery(accountId, pageRequest));

            var response = result.Adapt<GetPurchasedSoftwaresByAccountResponse>();

            return Results.Ok(response);
        })
        .WithName("GetPurchasedSoftwaresByAccount")
        .Produces<GetPurchasedSoftwaresByAccountResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get purchased softwares by account")
        .WithDescription("Get purchased softwares by account");
    }
}
