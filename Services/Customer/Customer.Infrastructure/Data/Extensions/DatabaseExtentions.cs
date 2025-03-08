using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Infrastructure.Data.Extensions;

public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(context);
    }
    
    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomers(context);
        await SeedAccounts(context);
        await SeedSoftwareLicenses(context);
    }

    private static async Task SeedCustomers(ApplicationDbContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedAccounts(ApplicationDbContext context)
    {
        if (!await context.Accounts.AnyAsync())
        {
            await context.Accounts.AddRangeAsync(InitialData.Accounts);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedSoftwareLicenses(ApplicationDbContext context)
    {
        if (!await context.SoftwareLicenses.AnyAsync())
        {
            await context.SoftwareLicenses.AddRangeAsync(InitialData.SoftwareLicenses);
            await context.SaveChangesAsync();
        }
    }
}
