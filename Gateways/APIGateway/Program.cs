using APIGateway;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var rateLimitingConfig = new RateLimitingConfig();
builder.Configuration.GetSection("RateLimiting").Bind(rateLimitingConfig);
var slidingPolicy = "sliding";

builder.Services.AddRateLimiter(_ => _
    .AddSlidingWindowLimiter(policyName: slidingPolicy, options =>
    {
        options.PermitLimit = rateLimitingConfig.PermitLimit;
        options.Window = TimeSpan.FromSeconds(rateLimitingConfig.Window);
        options.SegmentsPerWindow = rateLimitingConfig.SegmentsPerWindow;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = rateLimitingConfig.QueueLimit;
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRateLimiter();

app.MapReverseProxy();

app.Run();
