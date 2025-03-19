using Catalog.API.CCP;
using CCP.API.Models;
using CCP.API.Softwares.GetSoftwares;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace Catalog.API.Test.CCP;

public class SoftwareCatalogDownloaderTests
{
    private readonly Mock<ILogger<SoftwareCatalogDownloader>> loggerMock;
    private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
    private readonly IOptions<DownloadCatalogConfig> downloadCatalogConfigMock;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public SoftwareCatalogDownloaderTests()
    {
        loggerMock = new Mock<ILogger<SoftwareCatalogDownloader>>();
        httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        downloadCatalogConfigMock = Options.Create(new DownloadCatalogConfig { RequestUrl = "http://test.com" });
        jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    [Fact]
    public async Task DownloadSoftwareCatalogAsync_ReturnsSoftwareCatalog()
    {
        IEnumerable<Software> softwares = new List<Software>
        {
            new("Test Software", "Test Vendor", 100m)
        };
        var softwareCatalog = new GetSoftwareCatalogResponse(softwares);

        var jsonResponse = JsonSerializer.Serialize(softwareCatalog, jsonSerializerOptions);
        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        var httpClient = new HttpClient(httpMessageHandlerMock.Object);
        var downloader = new SoftwareCatalogDownloader(loggerMock.Object, httpClient, downloadCatalogConfigMock, jsonSerializerOptions);

        var result = await downloader.DownloadSoftwareCatalogAsync(CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Software", result.First().SoftwareName);
        Assert.Equal("Test Vendor", result.First().Vendor);
        Assert.Equal(100m, result.First().PricePerMonth);
    }
}
