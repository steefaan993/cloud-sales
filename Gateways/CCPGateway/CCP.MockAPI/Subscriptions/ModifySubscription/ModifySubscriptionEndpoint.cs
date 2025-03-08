using Carter;

namespace CCP.API.Subscriptions.ModifySubscription;

public record ModifySubscriptionRequest(Guid SubscriptionId, int Quantity);

public record ModifySubscriptionResponse(Guid SubscriptionId, int Quantity);

public class ModifySubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ccp/subscriptions/{subscriptionId}/modify", (Guid subscriptionId, ModifySubscriptionRequest request) =>
        {
            return Results.Ok(new ModifySubscriptionResponse(subscriptionId, request.Quantity));
        })

        .WithName("ModifySubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Modify subscription - Mock")
        .WithDescription("Modify subscription - Mock");
    }
}
