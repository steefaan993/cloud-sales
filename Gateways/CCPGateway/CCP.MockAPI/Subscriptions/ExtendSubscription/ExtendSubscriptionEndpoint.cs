using BuildingBlocks.Converters;
using Carter;
using System.Text.Json.Serialization;

namespace CCP.API.Subscriptions.ExtendSubscription;

public record ExtendSubscriptionRequest(
    Guid SubscriptionId,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidFrom,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidTo,
    int ExtensionPeriodInMonths);
public record ExtendSubscriptionResponse(
    Guid SubscriptionId,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidFrom,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidTo);

public class ExtendSubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ccp/subscriptions/{subscriptionId}/extend", (Guid subscriptionId, ExtendSubscriptionRequest request) =>
        {
            return Results.Ok(new ExtendSubscriptionResponse(subscriptionId, request.ValidFrom, request.ValidTo.AddMonths(request.ExtensionPeriodInMonths)));
        })

        .WithName("ExtendSubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Extend subscription - Mock")
        .WithDescription("Extend subscription - Mock");
    }
}
