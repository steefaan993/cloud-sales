using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.FeatureManagement;

namespace Customer.Application.SoftwareLicenses.EventHandlers.Domain;

public class SoftwareLicenseExpiresSoonEventHandler
    (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<SoftwareLicenseExpiresSoonEventHandler> logger)
    : INotificationHandler<SoftwareLicenseExpiredEvent>
{
    public async Task Handle(SoftwareLicenseExpiredEvent softwareLicenseExpiredEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Software license with ID expires soon. License valid to {licenseValidTo}", softwareLicenseExpiredEvent.SoftwareLicense.ValidTo);

        if (await featureManager.IsEnabledAsync("NotificationDelivery"))
        {
            // TODO We need a job that will check if there is any subscription that ends soon. This check should be performed daily (for example at midnight)
            var subscriptionEndsSoonEvent = new SubscriptionEndsSoonEvent();
            await publishEndpoint.Publish(subscriptionEndsSoonEvent, cancellationToken);
        }
    }
}
