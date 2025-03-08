using Carter;

namespace CCP.API.Subscriptions.CancelSubscription;

public class CancelSubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ccp/subscriptions/{subscriptionId}/cancel", (Guid subscriptionId) =>
        {
            return Results.Ok();
        })

        .WithName("CancelSubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Cancel subscription - Mock")
        .WithDescription("Cancel subscription - Mock");
    }
}
