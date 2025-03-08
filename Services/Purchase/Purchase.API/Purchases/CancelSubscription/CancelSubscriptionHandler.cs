using BuildingBlocks.Messaging.Events;
using MassTransit;
using Purchase.API.CCP;

namespace Purchase.API.Purchases.CancelSubscription;

public record CancelSubscriptionCommand(Guid SubscriptionId)
    : ICommand<CancelSubscriptionResult>;

public record CancelSubscriptionResult(Guid SubscriptionId, bool Success);

internal class CancelSubscriptionHandler(CCPClient ccpClient, IPublishEndpoint publishEndpoint) : ICommandHandler<CancelSubscriptionCommand, CancelSubscriptionResult>
{
    public async Task<CancelSubscriptionResult> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken)
    {
        await ccpClient.CancelSubscriptionAsync(command.SubscriptionId, cancellationToken);

        SubscriptionCancelledEvent subscriptionCancelledEvent = new()
        {
            SubscriptionId = command.SubscriptionId
        };
        await publishEndpoint.Publish(subscriptionCancelledEvent, cancellationToken);

        return new CancelSubscriptionResult(command.SubscriptionId, true);
    }
}

