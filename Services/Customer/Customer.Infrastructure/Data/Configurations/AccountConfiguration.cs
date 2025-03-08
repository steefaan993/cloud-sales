using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasConversion(
                accountId => accountId.Value,
                dbId => AccountId.Of(dbId));

        builder.Property(a => a.Name)
                .HasMaxLength(30)
                .IsRequired();

        builder.Property(a => a.Department)
                .HasMaxLength(30)
                .IsRequired();

        builder.HasMany(a => a.SoftwareLicenses)
                .WithOne()
                .HasForeignKey(s => s.AccountId);

        builder.HasOne<Domain.Models.Customer>()
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .IsRequired();

        builder.HasIndex(a => a.Name);
        builder.HasIndex(a => new { a.Name, a.Department }) 
                .HasDatabaseName("IX_Accounts_Name_Department");
    }
}
