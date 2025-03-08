namespace Customer.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Domain.Models.Customer> Customers { get; }
    DbSet<Account> Accounts { get; }
    DbSet<SoftwareLicense> SoftwareLicenses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
