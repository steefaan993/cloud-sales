using BuildingBlocks.Messaging.Events;
using MassTransit;
using Purchase.API.CCP;
using Purchase.API.Dtos;

namespace Purchase.API.Purchases.ModifySubscription;

public record ModifySubscriptionRequest(Guid SubscriptionId, int Quantity);

public class ModifySubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/subscriptions/{subscriptionId}/modify", (Guid subscriptionId, ModifySubscriptionRequest request, BackgroundTaskQueue backgroundTaskQueue) =>
        {
            backgroundTaskQueue.PutTaskInQueue(async serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<ModifySubscriptionEndpoint>>();
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var ccpClient = scope.ServiceProvider.GetRequiredService<CCPClient>();
                    var cancellationTokenSource = new CancellationTokenSource();
                    var cancellationToken = cancellationTokenSource.Token;
                    var result = await ccpClient.ModifySubscriptionAsync(request.Adapt<CCPModifySubscriptionRequest>(), cancellationToken);
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                    await publishEndpoint.Publish(
                        new SubscriptionModifiedEvent()
                        {
                            SubscriptionId = result.SubscriptionId,
                            Quantity = result.Quantity
                        }, cancellationToken);
                    }
                catch (Exception ex)
                {
                    logger?.LogError("Failed to change software license quantity: {exceptionMessage}", ex.Message);
                }
            });

            return Results.Ok();
        })

        .WithName("ModifySubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Modify subscription")
        .WithDescription("Modify subscription");
    }
}
