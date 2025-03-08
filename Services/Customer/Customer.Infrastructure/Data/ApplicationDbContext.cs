using Customer.Application.Data;
using System.Reflection;

namespace Customer.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Domain.Models.Customer> Customers => Set<Domain.Models.Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<SoftwareLicense> SoftwareLicenses => Set<SoftwareLicense>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        // Use Query filter to return only active licenses if it is required
        /*builder.Entity<SoftwareLicense>()
            .HasQueryFilter(sl => sl.State == Domain.Enums.SubscriptionState.Active);*/
    }
}
