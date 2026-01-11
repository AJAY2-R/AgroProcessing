using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class RawInventoryConfiguration : IEntityTypeConfiguration<RawInventory>
    {
        public void Configure(EntityTypeBuilder<RawInventory> builder)
        {
            builder.HasKey(ri => ri.RawInventoryId);
            builder.Property(ri => ri.Quantity)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.HasOne<PurchaseBatch>()
                   .WithMany()
                   .HasForeignKey(ri => ri.PurchaseBatchId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Product>()
                   .WithMany()
                   .HasForeignKey(ri => ri.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Location>()
                   .WithMany()
                   .HasForeignKey(ri => ri.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
