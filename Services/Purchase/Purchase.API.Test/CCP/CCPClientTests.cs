using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Purchase.API.CCP;
using Purchase.API.Dtos;

namespace Purchase.API.Test.CCP;

public class CCPClientTests
{
    private readonly Mock<ILogger<CCPClient>> loggerMock;
    private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
    private readonly IOptions<CCPClientConfig> downloadCatalogConfigMock;
    private readonly JsonSerializerOptions jsonOptions;

    public CCPClientTests()
    {
        loggerMock = new Mock<ILogger<CCPClient>>();
        httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        downloadCatalogConfigMock = Options.Create(new CCPClientConfig { ServerBaseUrl = "http://test.com" });
        jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    [Fact]
    public async Task PurchaseSoftwareAsync_ReturnsPurchaseSoftwareResult()
    {
        var request = new CCPPurchaseSoftwareRequest("Test Software", "Test Vendor", 12, 1);
        var purchaseResult = new PurchaseSoftwareResult(Guid.NewGuid(), "Test Software", "Test Vendor", DateTime.Now, DateTime.Now.AddMonths(12), 1);

        var jsonResponse = JsonSerializer.Serialize(purchaseResult, jsonOptions);
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
        var client = new CCPClient(loggerMock.Object, httpClient, downloadCatalogConfigMock, jsonOptions);

        var result = await client.PurchaseSoftwareAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Test Software", result.SoftwareName);
        Assert.Equal("Test Vendor", result.Vendor);
        Assert.Equal(DateTime.Now.Date, result.ValidFrom.Date);
        Assert.Equal(DateTime.Now.AddMonths(12).Date, result.ValidTo.Date);
        Assert.Equal(1, result.Quantity);
    }

    [Fact]
    public async Task ModifySubscriptionAsync_ReturnsModifySubscriptionResult()
    {
        var request = new CCPModifySubscriptionRequest(Guid.NewGuid(), 5);
        var modifyResult = new ModifySubscriptionResult(request.SubscriptionId, 5);

        var jsonResponse = JsonSerializer.Serialize(modifyResult, jsonOptions);
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
        var client = new CCPClient(loggerMock.Object, httpClient, downloadCatalogConfigMock, jsonOptions);

        var result = await client.ModifySubscriptionAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(request.SubscriptionId, result.SubscriptionId);
        Assert.Equal(5, result.Quantity);
    }

    [Fact]
    public async Task ExtendSubscriptionAsync_ReturnsExtendSubscriptionResult()
    {
        var request = new CCPExtendSubscriptionRequest(Guid.NewGuid(), new DateTime(2024, 10, 5), new DateTime(2025, 1, 4), 3);
        var extendResult = new ExtendSubscriptionResult(request.SubscriptionId, request.ValidFrom, request.ValidTo.AddMonths(request.ExtensionPeriodInMonths));

        var jsonResponse = JsonSerializer.Serialize(extendResult, jsonOptions);
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
        var client = new CCPClient(loggerMock.Object, httpClient, downloadCatalogConfigMock, jsonOptions);

        var result = await client.ExtendSubscriptionAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(request.SubscriptionId, result.SubscriptionId);
        Assert.Equal(new DateTime(2024, 10, 5).Date, result.ValidFrom.Date);
        Assert.Equal(new DateTime(2025, 1, 4).AddMonths(3).Date, result.ValidTo.Date);
    }
}