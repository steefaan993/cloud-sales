using Customer.Application.Accounts.Queries.GetOrdersByCustomer;
using BuildingBlocks.Pagination;

namespace Customer.API.Endpoints;

public record GetAccountsByCustomerRequest(GetAccountsByCustomerQuery GetAccountsByCustomerQuery);
public record GetAccountsByCustomerResponse(Guid CustomerId, PageResult<AccountData> Accounts);

public class GetAccountsByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/customers/{customerId}/accounts", async (Guid customerId, [AsParameters] PageRequest pageRequest, ISender sender) =>
        {
            var result = await sender.Send(new GetAccountsByCustomerQuery(customerId, pageRequest));

            var response = result.Adapt<GetAccountsByCustomerResponse>();

            return Results.Ok(response);
        })

        .WithName("GetAccountsByCustomer")
        .Produces<GetAccountsByCustomerResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get accounts by customer")
        .WithDescription("Get accounts by customer");

    }
}
