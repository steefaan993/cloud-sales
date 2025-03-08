using BuildingBlocks.Pagination;
using Catalog.API.CCP;
using Catalog.API.Extensions;
using System.Collections.ObjectModel;

namespace Catalog.API.Softwares.GetSoftwares;

public record GetSoftwaresQuery(PageRequest PageRequest) : IQuery<GetSoftwaresResult>;

public record GetSoftwaresResult(PageResult<Software> Softwares);

public class GetSoftwaresHandler
    (IDocumentStore store, SoftwareCatalogDownloader softwareCatalogDownloader)
    : IQueryHandler<GetSoftwaresQuery, GetSoftwaresResult>
{
    public async Task<GetSoftwaresResult> Handle(GetSoftwaresQuery query, CancellationToken cancellationToken)
    {
        using var session = store.LightweightSession();
        var numberOfSoftwaresInCatalog = await session.Query<Software>().CountAsync(cancellationToken);
        if (numberOfSoftwaresInCatalog == 0)
        {
            var softwareCatalog = await softwareCatalogDownloader.DownloadSoftwareCatalogAsync(cancellationToken);
            if (softwareCatalog.Any())
            {
                await store.BulkInsertAsync(new ReadOnlyCollection<Software>(SoftwareExtensions.ToSoftwareList(softwareCatalog)), cancellation: cancellationToken);
                await session.SaveChangesAsync(cancellationToken);
            }
        }
        var softwares = await session.Query<Software>().ToPagedListAsync(query.PageRequest.Page + 1, query.PageRequest.Size, cancellationToken);

        return new GetSoftwaresResult(new PageResult<Software>(query.PageRequest.Page, query.PageRequest.Size, softwares.TotalItemCount, softwares));
    }
}