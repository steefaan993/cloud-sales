namespace Purchase.API.CCP;

public class BackgroundWorker(IServiceProvider serviceProvider, BackgroundTaskQueue queue) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private readonly BackgroundTaskQueue queue = queue;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var task = queue.GetTaskFromQueue();
            if (task is not null)
            {
                await task(serviceProvider);
            }

            await Task.Delay(200, cancellationToken);
        }
    }
}
