using BuildingBlocks.Messaging.Events;
using MassTransit;
using Purchase.API.CCP;
using Purchase.API.Dtos;

namespace Purchase.API.Purchases.CancelSubscription;

public class CancelSubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/subscriptions/{subscriptionId}/cancel", (Guid subscriptionId, BackgroundTaskQueue backgroundTaskQueue) =>
        {
            backgroundTaskQueue.PutTaskInQueue(async serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<CancelSubscriptionEndpoint>>();
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var ccpClient = scope.ServiceProvider.GetRequiredService<CCPClient>();
                    var cancellationTokenSource = new CancellationTokenSource();
                    var cancellationToken = cancellationTokenSource.Token;
                    await ccpClient.CancelSubscriptionAsync(subscriptionId, cancellationToken);
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                    await publishEndpoint.Publish(
                        new SubscriptionCancelledEvent()
                        {
                            SubscriptionId = subscriptionId
                        }, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger?.LogError("Failed to cancel subscription: {exceptionMessage}", ex.Message);
                }
            });

            return Results.Ok();
        })

        .WithName("CancelSubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Cancel subscription")
        .WithDescription("Cancel subscription");
    }
}
