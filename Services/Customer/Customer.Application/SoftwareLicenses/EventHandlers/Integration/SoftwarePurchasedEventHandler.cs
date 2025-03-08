using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Customer.Application.SoftwareLicenses.EventHandlers.Integration;

public class SoftwarePurchasedEventHandler
    (IApplicationDbContext dbContext, ILogger<SoftwarePurchasedEventHandler> logger)
    : IConsumer<SoftwarePurchasedEvent>
{
    public async Task Consume(ConsumeContext<SoftwarePurchasedEvent> context)
    {
        var softwarePurchasedEvent = context.Message;

        logger.LogInformation("Software purchased event handled - account ID: {accountId}", softwarePurchasedEvent.AccountId);
        
        var account = await dbContext.Accounts.FindAsync(AccountId.Of(softwarePurchasedEvent.AccountId)) ?? throw new AccountNotFoundException(softwarePurchasedEvent.AccountId);
        account.AddSoftwareLicense(
            softwarePurchasedEvent.Vendor,
            softwarePurchasedEvent.SoftwareName,
            softwarePurchasedEvent.Quantity,
            softwarePurchasedEvent.SubscriptionId,
            softwarePurchasedEvent.ValidFrom,
            softwarePurchasedEvent.ValidTo);

        dbContext.Accounts.Update(account);
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
