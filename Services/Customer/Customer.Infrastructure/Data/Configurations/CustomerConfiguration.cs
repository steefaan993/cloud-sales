using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Domain.Models.Customer>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion(
                customerId => customerId.Value,
                dbId => CustomerId.Of(dbId));

        builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

        builder.Property(c => c.CompanyRegistrationNumber)
                .HasMaxLength(30)
                .IsRequired();

        builder.Property(c => c.Email)
                .HasMaxLength(320)
                .IsRequired();

        builder.Property(c => c.PhoneNumber).HasMaxLength(25);

        builder.ComplexProperty(
           c => c.Address, addressBuilder =>
           {
               addressBuilder.Property(a => a.StreetAddress)
                   .HasMaxLength(180)
                   .IsRequired();

               addressBuilder.Property(a => a.Country)
                   .HasMaxLength(50);

               addressBuilder.Property(a => a.City)
                   .HasMaxLength(50);

               addressBuilder.Property(a => a.PostalCode)
                   .HasMaxLength(5)
                   .IsRequired();
           });

        builder.HasMany(c => c.Accounts)
                .WithOne()
                .HasForeignKey(a => a.CustomerId);

        builder.HasOne(c => c.Reseller)
                   .WithMany()
                   .HasForeignKey(c => c.ResellerId)
                   .IsRequired(false);

        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.CompanyRegistrationNumber).IsUnique();
        builder.HasIndex(c => c.Email).IsUnique();
    }
}
