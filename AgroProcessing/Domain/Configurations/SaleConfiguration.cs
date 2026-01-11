using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.SaleId);
            builder.Property(s => s.SaleDate)
                   .IsRequired();
            builder.Property(s => s.TotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();
            builder.Property(s => s.Status)
                   .HasMaxLength(50)
                   .IsRequired()
                   .HasDefaultValue("Open");
            builder.HasOne<Buyer>()
                   .WithMany()
                   .HasForeignKey(s => s.BuyerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
