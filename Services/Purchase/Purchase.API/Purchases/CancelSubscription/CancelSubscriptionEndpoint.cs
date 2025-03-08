namespace Purchase.API.Purchases.CancelSubscription;

public class CancelSubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/subscriptions/{subscriptionId}/cancel", (Guid subscriptionId, ISender sender, IServiceProvider serviceProvider) =>
        {
            Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(new CancelSubscriptionCommand(subscriptionId));
            });

            return Results.Ok();
        })

        .WithName("CancelSubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Cancel subscription")
        .WithDescription("Cancel subscription");
    }
}
