using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Customer.Application.SoftwareLicenses.EventHandlers.Integration;

public class SubscriptionExtendedEventHandler
    (IApplicationDbContext dbContext, ILogger<SubscriptionExtendedEventHandler> logger)
    : IConsumer<SubscriptionExtendedEvent>
{
    public async Task Consume(ConsumeContext<SubscriptionExtendedEvent> context)
    {
        var subscriptionExtendedEvent = context.Message;

        logger.LogInformation("Subscription extended event handled - subscription ID: {subscriptionId}", subscriptionExtendedEvent.SubscriptionId);
        
        var softwareLicense = await dbContext.SoftwareLicenses
            .Where(sl => sl.ReferenceId == subscriptionExtendedEvent.SubscriptionId)
            .FirstOrDefaultAsync() ?? throw new SoftwareLicenseNotFoundException(subscriptionExtendedEvent.SubscriptionId);
        
        softwareLicense.Extend(subscriptionExtendedEvent.ValidTo);
        
        dbContext.SoftwareLicenses.Update(softwareLicense);
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
