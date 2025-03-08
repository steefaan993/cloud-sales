using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
namespace Catalog.API.CCP;

public class DownloadCatalogRetryPolicyProvider(ILogger<DownloadCatalogRetryPolicyProvider> logger, IOptions<DownloadCatalogRetryPolicyOptions> options)
{
    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                options.Value.RetryCount, // Max retry count
                retryAttempt => TimeSpan.FromSeconds(options.Value.RetryPauseSeconds),
                (result, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning($"Download software catalog - Retry attempt {retryCount}/{options.Value.RetryCount} after {timeSpan} due to {result.Exception?.Message ?? result.Result.StatusCode.ToString()}");
                });
    }

    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                options.Value.CircuitBreakerFailureCount, // Break circuit after 5 failures
                TimeSpan.FromSeconds(options.Value.CircuitBreakerResetSeconds), // Wait 30s before retrying
                onBreak: (result, timespan) =>
                {
                    logger.LogError($"Circuit broken for {timespan.TotalSeconds} seconds due to {result.Exception?.Message ?? result.Result.StatusCode.ToString()}");
                },
                onReset: () => logger.LogInformation("Circuit reset, normal operation resumed"),
                onHalfOpen: () => logger.LogInformation("Circuit in half-open state, allowing test requests"));
    }
}

public class DownloadCatalogRetryPolicyOptions
{
    public int RetryCount { get; set; } = 3;
    public int RetryPauseSeconds { get; set; } = 5;
    public int CircuitBreakerFailureCount { get; set; } = 5;
    public int CircuitBreakerResetSeconds { get; set; } = 30;
}
