using BuildingBlocks.Converters;
using Microsoft.Extensions.Options;
using Purchase.API.Dtos;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Purchase.API.CCP;

public record ExtendSubscriptionResult(
    Guid SubscriptionId,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidFrom,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidTo);

public record ModifySubscriptionResult(Guid SubscriptionId, int Quantity);

public record PurchaseSoftwareResult(
    Guid SubscriptionId,
    string SoftwareName,
    string Vendor,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidFrom,
    [property: JsonConverter(typeof(DateTimeConverter))] DateTime ValidTo,
    int Quantity);

public class CCPClient(ILogger<CCPClient> logger, HttpClient httpClient, IOptions<CCPClientConfig> options, JsonSerializerOptions jsonOptions)
{
    public async Task<PurchaseSoftwareResult> PurchaseSoftwareAsync(CCPPurchaseSoftwareRequest request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Purchase software started - vendor: {vendor}, software: {software}, number of licenses: {quantity}",
                request.Vendor, request.SoftwareName, request.Quantity);

            var content = new StringContent(JsonSerializer.Serialize(request, jsonOptions), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{options.Value.ServerBaseUrl}/purchase", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var jsonStringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            var purchaseSoftwareResponse = JsonSerializer.Deserialize<PurchaseSoftwareResult>(jsonStringResponse, jsonOptions) ?? throw new JsonException("Failed to deserialize data");
            logger.LogInformation("Software purchase completed");

            return purchaseSoftwareResponse;
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to purchase software");
            logger.LogError("Failed to purchase software: {exceptionMessage}", ex.Message);

            throw;
        }
    }

    public async Task CancelSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Cancel subscription with ID {subscriptionId} started", subscriptionId);

            var response = await httpClient.PostAsync($"{options.Value.ServerBaseUrl}/subscriptions/{subscriptionId}/cancel", new StringContent(string.Empty), cancellationToken);
            response.EnsureSuccessStatusCode();

            logger.LogInformation("Subscription with ID {subscriptionId} cancelled", subscriptionId);
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to cancel subscription");
            logger.LogError("Failed to cancel subscription: {exceptionMessage}", ex.Message);

            throw;
        }
    }

    public async Task<ModifySubscriptionResult> ModifySubscriptionAsync(CCPModifySubscriptionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Modify subscription with ID {subscriptionId} started", request.SubscriptionId);

            var content = new StringContent(JsonSerializer.Serialize(request, jsonOptions), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{options.Value.ServerBaseUrl}/subscriptions/{request.SubscriptionId}/modify", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var jsonStringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            var modifySubcriptionResponse = JsonSerializer.Deserialize<ModifySubscriptionResult>(jsonStringResponse, jsonOptions) ?? throw new JsonException("Failed to deserialize data"); ;

            logger.LogInformation("Subscription with ID {subscriptionId} modified", request.SubscriptionId);

            return modifySubcriptionResponse;
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to modify subscription");
            logger.LogError("Failed to modify subscription: {exceptionMessage}", ex.Message);

            throw;
        }
    }

    public async Task<ExtendSubscriptionResult> ExtendSubscriptionAsync(CCPExtendSubscriptionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Extend subscription with ID {subscriptionId} started", request.SubscriptionId);

            var content = new StringContent(JsonSerializer.Serialize(request, jsonOptions), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{options.Value.ServerBaseUrl}/subscriptions/{request.SubscriptionId}/extend", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var jsonStringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            var extendSubcriptionResponse = JsonSerializer.Deserialize<ExtendSubscriptionResult>(jsonStringResponse, jsonOptions) ?? throw new JsonException("Failed to deserialize data"); ;

            logger.LogInformation("Subscription with ID {subscriptionId} extended", request.SubscriptionId);

            return extendSubcriptionResponse;
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to extend subscription");
            logger.LogError("Failed to extend subscription: {exceptionMessage}", ex.Message);

            throw;
        }
    }
}

public class CCPClientConfig
{
    public string ServerBaseUrl { get; set; } = default!;
}
