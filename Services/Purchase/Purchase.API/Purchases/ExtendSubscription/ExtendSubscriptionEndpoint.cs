using BuildingBlocks.Converters;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using MassTransit.Transports;
using Purchase.API.CCP;
using Purchase.API.Dtos;
using System.Text.Json.Serialization;
using static MassTransit.ValidationResultExtensions;

namespace Purchase.API.Purchases.ExtendSubscription;

public record ExtendSubscriptionRequest(
    Guid SubscriptionId,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidFrom,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidTo,
    int ExtensionPeriodInMonths);

public class ExtendSubscriptionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/subscriptions/{subscriptionId}/extend", (Guid subscriptionId, ExtendSubscriptionRequest request, BackgroundTaskQueue backgroundTaskQueue) =>
        {
            backgroundTaskQueue.PutTaskInQueue(async serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<ExtendSubscriptionEndpoint>>();
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var ccpClient = scope.ServiceProvider.GetRequiredService<CCPClient>();
                    var cancellationTokenSource = new CancellationTokenSource();
                    var cancellationToken = cancellationTokenSource.Token;
                    var result = await ccpClient.ExtendSubscriptionAsync(request.Adapt<CCPExtendSubscriptionRequest>(), cancellationToken);
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                    await publishEndpoint.Publish(
                        new SubscriptionExtendedEvent()
                        {
                            SubscriptionId = result.SubscriptionId,
                            ValidFrom = result.ValidFrom,
                            ValidTo = result.ValidTo,
                        }, cancellationToken);
                    }
                catch (Exception ex)
                {
                    logger?.LogError("Failed to extend software license: {exceptionMessage}", ex.Message);
                }
            });

            return Results.Ok();
        })

        .WithName("ExtendSubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Extend subscription")
        .WithDescription("Extend subscription");
    }
}
