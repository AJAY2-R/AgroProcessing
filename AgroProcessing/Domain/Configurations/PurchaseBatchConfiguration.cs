using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class PurchaseBatchConfiguration : IEntityTypeConfiguration<PurchaseBatch>
    {
        public void Configure(EntityTypeBuilder<PurchaseBatch> builder)
        {
            builder.HasKey(pb => pb.PurchaseBatchId);
            builder.Property(pb => pb.PurchaseDate)
                   .IsRequired();
            builder.Property(pb => pb.RawWeight)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(pb => pb.RatePerKg)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(pb => pb.TotalAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(pb => pb.PaymentSettlementType)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(pb => pb.Status)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("Stored");
            builder.HasOne<Farmer>()
                   .WithMany()
                   .HasForeignKey(pb => pb.FarmerId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Product>()
                   .WithMany()
                   .HasForeignKey(pb => pb.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(pb => pb.Payments)
                   .WithOne()
                   .HasForeignKey(pp => pp.PurchaseBatchId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
