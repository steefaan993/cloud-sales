namespace CCP.API.Subscriptions.ModifySubscription;

public record ModifySubscriptionRequest(Guid SubscriptionId, int Quantity);

public class ModifySubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/subscriptions/{subscriptionId}/modify", (Guid subscriptionId, ModifySubscriptionRequest request, IServiceProvider serviceProvider) =>
        {
            Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(request.Adapt<ModifySubscriptionCommand>());
            });

            return Results.Ok();
        })

        .WithName("ModifySubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Modify subscription")
        .WithDescription("Modify subscription");
    }
}
