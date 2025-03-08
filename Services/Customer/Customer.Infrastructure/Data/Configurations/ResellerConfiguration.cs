using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Data.Configurations;

public class ResellerConfiguration : IEntityTypeConfiguration<Reseller>
{
    public void Configure(EntityTypeBuilder<Reseller> builder)
    {
        builder.ToTable("Resellers");

        builder.Property(r => r.CommissionPercentage)
                .IsRequired(false);

        builder.Property(r => r.BaseDiscountRate)
                .IsRequired(false);
    }
}
