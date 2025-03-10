namespace Purchase.API.Purchases.PurchaseSoftware;

public record PurchaseSoftwareRequest(Guid CustomerId, Guid AccountId, string SoftwareName, string Vendor, int PeriodInMonths, int Quantity);

public class PurchaseSoftwareEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/purchase", async (PurchaseSoftwareRequest request, IServiceProvider serviceProvider) =>
        {
            var purchaseSwTask = Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(request.Adapt<PurchaseSoftwareCommand>());
            });

            await Task.WhenAny(purchaseSwTask, Task.Delay(200));

            return Results.Ok();
        })

        .WithName("PurchaseSoftware")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Purchase software")
        .WithDescription("Purchase software");
    }
}
