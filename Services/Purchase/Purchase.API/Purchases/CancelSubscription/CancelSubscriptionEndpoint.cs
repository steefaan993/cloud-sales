namespace Purchase.API.Purchases.CancelSubscription;

public class CancelSubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/subscriptions/{subscriptionId}/cancel", async (Guid subscriptionId, IServiceProvider serviceProvider) =>
        {
            var cancelSubTask = Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(new CancelSubscriptionCommand(subscriptionId));
            });

            await Task.WhenAny(cancelSubTask, Task.Delay(200));

            return Results.Ok();
        })

        .WithName("CancelSubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Cancel subscription")
        .WithDescription("Cancel subscription");
    }
}
