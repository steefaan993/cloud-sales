using BuildingBlocks.Messaging.Events;
using MassTransit;
using Purchase.API.CCP;
using Purchase.API.Dtos;

namespace Purchase.API.Purchases.PurchaseSoftware;

public record PurchaseSoftwareCommand(Guid CustomerId, Guid AccountId, string SoftwareName, string Vendor, int PeriodInMonths, int Quantity)
    : ICommand<PurchaseSoftwareResult>;

internal class PurchaseSoftwareEndpointHandler(CCPClient ccpClient, IPublishEndpoint publishEndpoint) : ICommandHandler<PurchaseSoftwareCommand, PurchaseSoftwareResult>
{   
    public async Task<PurchaseSoftwareResult> Handle(PurchaseSoftwareCommand command, CancellationToken cancellationToken)
    {
        var result = await ccpClient.PurchaseSoftwareAsync(command.Adapt<CCPPurchaseSoftwareRequest>(), cancellationToken);

        await publishEndpoint.Publish(MapToSoftwarePurchasedEvent(command.AccountId, result), cancellationToken);

        return result;
    }

    private static SoftwarePurchasedEvent MapToSoftwarePurchasedEvent(Guid accountId, PurchaseSoftwareResult result)
    {
        return new()
        {
            AccountId = accountId,
            SubscriptionId = result.SubscriptionId,
            SoftwareName = result.SoftwareName,
            Vendor = result.Vendor,
            ValidFrom = result.ValidFrom,
            ValidTo = result.ValidTo,
            Quantity = result.Quantity
        };
    }
}
