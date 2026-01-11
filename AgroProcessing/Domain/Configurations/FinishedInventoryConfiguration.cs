using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class FinishedInventoryConfiguration : IEntityTypeConfiguration<FinishedInventory>
    {
        public void Configure(EntityTypeBuilder<FinishedInventory> builder)
        {
            builder.HasKey(fi => fi.FinishedInventoryId);
            builder.Property(fi => fi.Quantity)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.HasOne<PurchaseBatch>()
                   .WithMany()
                   .HasForeignKey(fi => fi.PurchaseBatchId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Product>()
                   .WithMany()
                   .HasForeignKey(fi => fi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Location>()
                   .WithMany()
                   .HasForeignKey(fi => fi.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
