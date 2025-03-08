using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Customer.Application.SoftwareLicenses.EventHandlers.Integration;

public class SubscriptionModifiedEventHandler
    (IApplicationDbContext dbContext, ILogger<SubscriptionModifiedEventHandler> logger)
    : IConsumer<SubscriptionModifiedEvent>
{
    public async Task Consume(ConsumeContext<SubscriptionModifiedEvent> context)
    {
        var subscriptionExtendedEvent = context.Message;

        logger.LogInformation("Subscription modified event handled - subscription ID: {subscriptionId}", subscriptionExtendedEvent.SubscriptionId);

        var softwareLicense = await dbContext.SoftwareLicenses
            .Where(sl => sl.ReferenceId == subscriptionExtendedEvent.SubscriptionId)
            .FirstOrDefaultAsync() ?? throw new SoftwareLicenseNotFoundException(subscriptionExtendedEvent.SubscriptionId);

        softwareLicense.Modify(subscriptionExtendedEvent.Quantity);

        dbContext.SoftwareLicenses.Update(softwareLicense);
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
