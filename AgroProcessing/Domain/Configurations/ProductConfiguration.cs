using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(p => p.ExpectedYieldPercent)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");
            builder.Property(p => p.DryingDaysMin)
                   .IsRequired();
            builder.Property(p => p.DryingDaysMax)
                   .IsRequired();
            builder.Property(p => p.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}
