using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.HasKey(si => si.SaleItemId);
            builder.Property(si => si.Quantity)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(si => si.Rate)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.HasOne<Sale>()
                   .WithMany()
                   .HasForeignKey(si => si.SaleId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<Product>()
                   .WithMany()
                   .HasForeignKey(si => si.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
