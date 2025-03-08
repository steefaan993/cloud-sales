using BuildingBlocks.Messaging.Events;
using MassTransit;
using Purchase.API.CCP;
using Purchase.API.Dtos;

namespace Purchase.API.Purchases.ModifySubscription;

public record ModifySubscriptionCommand(Guid SubscriptionId, int Quantity)
    : ICommand<ModifySubscriptionResult>;

internal class ModifySubscriptionHandler(CCPClient ccpClient, IPublishEndpoint publishEndpoint) : ICommandHandler<ModifySubscriptionCommand, ModifySubscriptionResult>
{
    public async Task<ModifySubscriptionResult> Handle(ModifySubscriptionCommand command, CancellationToken cancellationToken)
    {
        var result = await ccpClient.ModifySubscriptionAsync(command.Adapt<CCPModifySubscriptionRequest>(), cancellationToken);
        
        await publishEndpoint.Publish(MapToSoftwarePurchasedEvent(result), cancellationToken);

        return result;
    }

    private static SubscriptionModifiedEvent MapToSoftwarePurchasedEvent(ModifySubscriptionResult result)
    {
        return new()
        {
            SubscriptionId = result.SubscriptionId,
            Quantity = result.Quantity
        };
    }
}
