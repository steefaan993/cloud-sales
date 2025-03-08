using Catalog.API.Extensions;
using Quartz;
using System.Collections.ObjectModel;

namespace Catalog.API.CCP;

[DisallowConcurrentExecution]
public class UpdateSoftwareCatalogJob(ILogger<UpdateSoftwareCatalogJob> logger, SoftwareCatalogDownloader softwareCatalogDownloader, IDocumentStore store) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            logger.LogInformation("Update software catalog started");
            
            var cancellationToken = context.CancellationToken;

            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogInformation("Update software catalog task has been cancelled");
                return;
            }

            var softwareCatalog = await softwareCatalogDownloader.DownloadSoftwareCatalogAsync(cancellationToken);
            if (softwareCatalog.Any())
            {
                using var session = store.LightweightSession();
                session.DeleteWhere<Software>(s => true);
                await store.BulkInsertAsync(new ReadOnlyCollection<Software>(SoftwareExtensions.ToSoftwareList(softwareCatalog)), cancellation: cancellationToken);
                await session.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Update software catalog finished");
            }
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to update software catalog");
            logger.LogError("Failed to update software catalog: {exceptionMessage}", ex.Message);
        }   
    }
}
