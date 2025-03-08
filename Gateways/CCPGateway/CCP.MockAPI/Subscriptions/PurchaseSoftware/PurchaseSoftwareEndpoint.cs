using BuildingBlocks.Converters;
using Carter;
using System.Text.Json.Serialization;

namespace CCP.API.Subscriptions.PurchaseSoftware;

public record PurchaseSoftwareRequest(string SoftwareName, string Vendor, int PeriodInMohtns, int Quantity);

public record PurchaseSoftwareResponse(
    Guid SubscriptionId,
    string SoftwareName,
    string Vendor,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidFrom,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidTo,
    int Quantity);

public class PurchaseSoftwareEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ccp/purchase", (PurchaseSoftwareRequest request) =>
        {
            return Results.Ok(new PurchaseSoftwareResponse(Guid.NewGuid(), request.SoftwareName, request.Vendor, DateTime.Now, DateTime.Now.AddMonths(request.PeriodInMohtns), request.Quantity));
        })

        .WithName("PurchaseSoftware")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Purchase software - Mock")
        .WithDescription("Purchase software - Mock");
    }
}
