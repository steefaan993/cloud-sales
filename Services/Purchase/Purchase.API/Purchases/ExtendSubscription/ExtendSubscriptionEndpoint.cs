using BuildingBlocks.Converters;
using System.Text.Json.Serialization;

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
        app.MapPost("/subscriptions/{subscriptionId}/extend", async (Guid subscriptionId, ExtendSubscriptionRequest request, IServiceProvider serviceProvider) =>
        {
            var extendSubTask = Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(request.Adapt<ExtendSubscriptionCommand>());
            });

            await Task.WhenAny(extendSubTask, Task.Delay(200));

            return Results.Ok();
        })

        .WithName("ExtendSubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Extend subscription")
        .WithDescription("Extend subscription");
    }
}
