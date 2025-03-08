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
        app.MapPost("/subscriptions/{subscriptionId}/extend", (Guid subscriptionId, ExtendSubscriptionRequest request, IServiceProvider serviceProvider) =>
        {
            Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(request.Adapt<ExtendSubscriptionCommand>());
            });

            return Results.Ok();
        })

        .WithName("ExtendSubscription")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Extend subscription")
        .WithDescription("Extend subscription");
    }
}
