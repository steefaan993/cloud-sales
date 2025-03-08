namespace Purchase.API.Purchases.PurchaseSoftware;

public record PurchaseSoftwareRequest(Guid CustomerId, Guid AccountId, string SoftwareName, string Vendor, int PeriodInMonths, int Quantity);

public class PurchaseSoftwareEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/purchase", (PurchaseSoftwareRequest request, IServiceProvider serviceProvider) =>
        {
            Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(request.Adapt<PurchaseSoftwareCommand>());
            });

            return Results.Ok();
        })

        .WithName("PurchaseSoftware")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Purchase software")
        .WithDescription("Purchase software");
    }
}
