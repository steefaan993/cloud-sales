using BuildingBlocks.Messaging.Events;
using MassTransit;
using MassTransit.Transports;
using Purchase.API.CCP;
using Purchase.API.Dtos;

namespace Purchase.API.Purchases.PurchaseSoftware;

public record PurchaseSoftwareRequest(Guid CustomerId, Guid AccountId, string SoftwareName, string Vendor, int PeriodInMonths, int Quantity);

public class PurchaseSoftwareEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/purchase", (PurchaseSoftwareRequest request, BackgroundTaskQueue backgroundTaskQueue) =>
        {
            backgroundTaskQueue.PutTaskInQueue(async serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<PurchaseSoftwareEndpoint>>();
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var ccpClient = scope.ServiceProvider.GetRequiredService<CCPClient>();
                    var cancellationTokenSource = new CancellationTokenSource();
                    var cancellationToken = cancellationTokenSource.Token;
                    var result = await ccpClient.PurchaseSoftwareAsync(new CCPPurchaseSoftwareRequest(request.SoftwareName, request.Vendor, request.PeriodInMonths, request.Quantity), cancellationToken);
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                    await publishEndpoint.Publish(
                        new SoftwarePurchasedEvent()
                        {
                            AccountId = request.AccountId,
                            SubscriptionId = result.SubscriptionId,
                            SoftwareName = result.SoftwareName,
                            Vendor = result.Vendor,
                            ValidFrom = result.ValidFrom,
                            ValidTo = result.ValidTo,
                            Quantity = result.Quantity
                        }, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger?.LogError("Failed to purchase software: {exceptionMessage}", ex.Message);
                }
            });

            return Results.Ok();
        })

        .WithName("PurchaseSoftware")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Purchase software")
        .WithDescription("Purchase software");
    }
}
