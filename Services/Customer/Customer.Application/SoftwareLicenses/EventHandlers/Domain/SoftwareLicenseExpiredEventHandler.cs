using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.FeatureManagement;

namespace Customer.Application.SoftwareLicenses.EventHandlers.Domain;

public class SoftwareLicenseExpiredEventHandler
    (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<SoftwareLicenseExpiredEventHandler> logger)
    : INotificationHandler<SoftwareLicenseExpiredEvent>
{
    public async Task Handle(SoftwareLicenseExpiredEvent softwareLicenseExpiredEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Software license with ID has expired: {domainEvent}", softwareLicenseExpiredEvent.SoftwareLicense.Id.Value);

        if (await featureManager.IsEnabledAsync("NotificationDelivery"))
        {
            // TODO We need a job that will check if there is any subscription that has been expired. This check should be performed daily (for example at midnight)
            var subscriptionEndedEvent = new SubscriptionEndedEvent();
            await publishEndpoint.Publish(subscriptionEndedEvent, cancellationToken);
        }
    }
}
