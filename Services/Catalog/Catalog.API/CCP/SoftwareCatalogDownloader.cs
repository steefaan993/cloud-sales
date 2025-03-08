using Catalog.API.Dtos;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Catalog.API.CCP;

public class SoftwareCatalogDownloader(ILogger<SoftwareCatalogDownloader> logger, HttpClient httpClient, IOptions<DownloadCatalogConfig> options, JsonSerializerOptions jsonOptions)
{
    public async Task<IEnumerable<SoftwareInformation>> DownloadSoftwareCatalogAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Download software catalog from CCP started");
            var response = await httpClient.GetAsync(options.Value.RequestUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            var jsonStringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            var softwareCatalog = JsonSerializer.Deserialize<SoftwareCatalog>(jsonStringResponse, jsonOptions);

            logger.LogInformation("Download software catalog from CCP finished");
            return softwareCatalog?.Softwares ?? [];
        }
        catch (Exception ex) 
        {
            logger.LogDebug(ex, "Failed to download software catalog from CCP");
            logger.LogError("Failed to download software catalog from CCP: {exceptionMessage}", ex.Message);
        }

        return [];
    }
}

public class DownloadCatalogConfig
{
    public string RequestUrl { get; set; } = default!;
}
