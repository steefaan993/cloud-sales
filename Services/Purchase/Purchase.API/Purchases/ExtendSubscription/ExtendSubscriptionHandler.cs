using BuildingBlocks.Messaging.Events;
using MassTransit;
using Purchase.API.CCP;
using Purchase.API.Dtos;

namespace Purchase.API.Purchases.ExtendSubscription;

public record ExtendSubscriptionCommand(Guid SubscriptionId, DateTime ValidFrom, DateTime ValidTo, int ExtensionPeriodInMonths)
    : ICommand<ExtendSubscriptionResult>;

internal class ExtendSubscriptionHandler(CCPClient ccpClient, IPublishEndpoint publishEndpoint) : ICommandHandler<ExtendSubscriptionCommand, ExtendSubscriptionResult>
{
    public async Task<ExtendSubscriptionResult> Handle(ExtendSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var result = await ccpClient.ExtendSubscriptionAsync(command.Adapt<CCPExtendSubscriptionRequest>(), cancellationToken);

        await publishEndpoint.Publish(MapToSoftwarePurchasedEvent(result), cancellationToken);

        return result;
    }

    private static SubscriptionExtendedEvent MapToSoftwarePurchasedEvent(ExtendSubscriptionResult result)
    {
        return new()
        {
            SubscriptionId = result.SubscriptionId,
            ValidFrom = result.ValidFrom,
            ValidTo = result.ValidTo,
        };
    }
}
