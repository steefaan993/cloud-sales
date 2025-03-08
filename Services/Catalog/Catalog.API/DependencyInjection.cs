using Catalog.API.CCP;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Quartz;
using System.Text.Json;

namespace Catalog.API;

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

        services.Configure<DownloadCatalogRetryPolicyOptions>(configuration.GetSection("DownloadSoftwareCatalogRetryPolicy"));
        services.Configure<DownloadCatalogConfig>(configuration.GetSection("DownloadSoftwareCatalog"));
        services.AddSingleton<DownloadCatalogRetryPolicyProvider>();
        services.AddHttpClient<SoftwareCatalogDownloader>()
                .AddPolicyHandler((serviceProvider, request) =>
                {
                    var policyProvider = serviceProvider.GetRequiredService<DownloadCatalogRetryPolicyProvider>();
                    return policyProvider.GetRetryPolicy();
                })
                .AddPolicyHandler((serviceProvider, request) =>
                {
                    var policyProvider = serviceProvider.GetRequiredService<DownloadCatalogRetryPolicyProvider>();
                    return policyProvider.GetCircuitBreakerPolicy();
                });
        services.AddScoped<SoftwareCatalogDownloader>();

        services.AddCarter();

        services.AddMarten(opts =>
        {
            opts.Connection(configuration.GetConnectionString("Database")!);
        }).UseLightweightSessions();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.AddScoped<UpdateSoftwareCatalogJob>();
        var updateSoftwareCatalogCronSchedule = configuration["UpdateSoftwareCatalog_CronSchedule"];

        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!);

        return services;
    }

    public static async Task<WebApplication> UseServices(this WebApplication app, IConfiguration configuration)
    {
        app.MapCarter();

        app.UseExceptionHandler(options => { });

        var updateSoftwareCatalogCronSchedule = configuration["UpdateSoftwareCatalog_CronSchedule"];

        var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();
        var job = JobBuilder.Create<UpdateSoftwareCatalogJob>()
            .WithIdentity("updateSoftwareCatalogJob")
            .Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity("updateSoftwareCatalogTrigger")
            .WithCronSchedule(updateSoftwareCatalogCronSchedule)
            .Build();
        await scheduler.ScheduleJob(job, trigger);

        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        return app;
    }
}
