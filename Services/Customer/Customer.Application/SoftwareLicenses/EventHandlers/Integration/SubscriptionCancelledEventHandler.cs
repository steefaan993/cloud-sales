using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Customer.Application.SoftwareLicenses.EventHandlers.Integration;

public class SubscriptionCancelledEventHandler
    (IApplicationDbContext dbContext, ILogger<SubscriptionCancelledEventHandler> logger)
    : IConsumer<SubscriptionCancelledEvent>
{
    public async Task Consume(ConsumeContext<SubscriptionCancelledEvent> context)
    {
        var subscriptionCancelledEvent = context.Message;

        logger.LogInformation("Subscription cancelled event handled - subscription ID: {subscriptionId}", subscriptionCancelledEvent.SubscriptionId);

        var softwareLicense = await dbContext.SoftwareLicenses
            .Where(sl => sl.ReferenceId == subscriptionCancelledEvent.SubscriptionId)
            .FirstOrDefaultAsync() ?? throw new SoftwareLicenseNotFoundException(subscriptionCancelledEvent.SubscriptionId);

        softwareLicense.Cancel();

        dbContext.SoftwareLicenses.Update(softwareLicense);
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
