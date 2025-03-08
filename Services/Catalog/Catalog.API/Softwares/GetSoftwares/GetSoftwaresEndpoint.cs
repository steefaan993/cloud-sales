using BuildingBlocks.Pagination;

namespace Catalog.API.Softwares.GetSoftwares;

public record GetSoftwaresResponse(PageResult<Software> Softwares);

public class GetSoftwaresEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/catalog/softwares", async ([AsParameters] PageRequest pageRequest, ISender sender) =>
        {
            var result = await sender.Send(new GetSoftwaresQuery(pageRequest));

            var response = result.Adapt<GetSoftwaresResponse>();

            return Results.Ok(response);
        })

        .WithName("GetSoftwareCatalog")
        .Produces<GetSoftwaresResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get software catalog")
        .WithDescription("Get software catalog");

    }
}
