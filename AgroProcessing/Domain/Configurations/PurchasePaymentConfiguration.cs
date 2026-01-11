using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class PurchasePaymentConfiguration : IEntityTypeConfiguration<PurchasePayment>
    {
        public void Configure(EntityTypeBuilder<PurchasePayment> builder)
        {
            builder.HasKey(pp => pp.PurchasePaymentId);
            builder.Property(pp => pp.PaymentDate);
            builder.Property(pp => pp.AmountPaid)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(pp => pp.DueDate)
                   .IsRequired();
            builder.Property(pp => pp.PaymentMode)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(pp => pp.PaymentStatus)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(pp => pp.Remarks)
                   .HasMaxLength(500);
            builder.HasOne<PurchaseBatch>()
                   .WithMany(pb => pb.Payments)
                   .HasForeignKey(pp => pp.PurchaseBatchId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
