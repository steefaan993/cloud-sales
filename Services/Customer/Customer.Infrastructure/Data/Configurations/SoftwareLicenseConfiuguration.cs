using Customer.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Data.Configurations;

public class SoftwareLicenseConfiguration : IEntityTypeConfiguration<SoftwareLicense>
{
    public void Configure(EntityTypeBuilder<SoftwareLicense> builder)
    {
        builder.HasKey(sl => sl.Id);
        builder.Property(sl => sl.Id).HasConversion(
                softwareId => softwareId.Value,
                dbId => SoftwareLicenseId.Of(dbId));

        builder.Property(sl => sl.Vendor)
                .HasMaxLength(100)
                .IsRequired();

        builder.Property(sl => sl.SoftwareName)
                .HasMaxLength(100)
                .IsRequired();

        builder.Property(sl => sl.Quantity)
                .IsRequired();

        builder.Property(sl => sl.State)
                .HasConversion(
                    State => State.ToString(),
                    dbState => (SubscriptionState)Enum.Parse(typeof(SubscriptionState), dbState))
                .IsRequired();

        builder.Property(sl => sl.ReferenceId)
                .IsRequired();

        builder.Property(sl => sl.ValidFrom)
                .HasConversion(
                    validFrom => validFrom.ToUniversalTime().Date,
                    dbValidFromDate => dbValidFromDate)
                .IsRequired();

        builder.Property(sl => sl.ValidTo)
                .HasConversion(
                    validTo => validTo.ToUniversalTime().Date,
                    dbValidToDate => dbValidToDate)
                .IsRequired();

        builder.HasIndex(sl => sl.Vendor);
        builder.HasIndex(sl => sl.SoftwareName);
        builder.HasIndex(sl => new { sl.Vendor, sl.SoftwareName })
                .HasDatabaseName("IX_SoftwareLicenses_Vendor_SoftwareName");
        builder.HasIndex(sl => sl.ReferenceId).IsUnique();

        /*builder.HasOne<Account>()
              .WithMany()
              .HasForeignKey(ss => ss.AccountId)
              .IsRequired();*/
    }
}
