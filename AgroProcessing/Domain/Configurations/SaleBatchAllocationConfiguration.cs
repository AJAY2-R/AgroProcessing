using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class SaleBatchAllocationConfiguration : IEntityTypeConfiguration<SaleBatchAllocation>
    {
        public void Configure(EntityTypeBuilder<SaleBatchAllocation> builder)
        {
            builder.HasKey(sba => sba.SaleBatchAllocationId);
            builder.Property(sba => sba.QuantityAllocated)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.HasOne<SaleItem>()
                   .WithMany()
                   .HasForeignKey(sba => sba.SaleItemId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<PurchaseBatch>()
                   .WithMany()
                   .HasForeignKey(sba => sba.PurchaseBatchId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
