using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Purchase.API.CCP;

public class PurchaseSoftwareRetryPolicyProvider(ILogger<PurchaseSoftwareRetryPolicyProvider> logger, IOptions<PurchaseSoftwareRetryPolicyOptions> options)
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
                    logger.LogWarning($"Purchase software - Retry attempt {retryCount}/{options.Value.RetryCount} after {timeSpan} due to {result.Exception?.Message ?? result.Result.StatusCode.ToString()}");
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

public class PurchaseSoftwareRetryPolicyOptions
{
    public int RetryCount { get; set; } = 5;
    public int RetryPauseSeconds { get; set; } = 10;
    public int CircuitBreakerFailureCount { get; set; } = 10;
    public int CircuitBreakerResetSeconds { get; set; } = 30;
}
