using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Purchase.API.CCP;
using BuildingBlocks.Messaging.MassTransit;
using System.Text.Json;

namespace Purchase.API;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(Program).Assembly;
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        services.AddValidatorsFromAssembly(assembly);

        services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        services.Configure<PurchaseSoftwareRetryPolicyOptions>(configuration.GetSection("PurchaseSoftwareRetryPolicy"));
        services.Configure<CCPClientConfig>(configuration.GetSection("CCPClient"));
        services.AddSingleton<PurchaseSoftwareRetryPolicyProvider>();
        services.AddHttpClient<CCPClient>()
                .AddPolicyHandler((serviceProvider, request) =>
                {
                    var policyProvider = serviceProvider.GetRequiredService<PurchaseSoftwareRetryPolicyProvider>();
                    return policyProvider.GetRetryPolicy();
                })
                .AddPolicyHandler((serviceProvider, request) =>
                {
                    var policyProvider = serviceProvider.GetRequiredService<PurchaseSoftwareRetryPolicyProvider>();
                    return policyProvider.GetCircuitBreakerPolicy();
                });
        services.AddScoped<CCPClient>();

        services.AddSingleton<BackgroundTaskQueue>();
        services.AddHostedService<BackgroundWorker>();

        services.AddCarter();

        services.AddMessageBroker(configuration);

        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddHealthChecks();

        return services;
    }

    public static async Task<WebApplication> UseServices(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(options => { });

        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        return app;
    }
}
