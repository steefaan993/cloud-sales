using Purchase.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

await app.UseServices();

app.Run();
