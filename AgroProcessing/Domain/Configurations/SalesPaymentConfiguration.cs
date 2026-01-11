using AgroProcessing.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroProcessing.Domain.Configurations
{
    public class SalesPaymentConfiguration : IEntityTypeConfiguration<SalesPayment>
    {
        public void Configure(EntityTypeBuilder<SalesPayment> builder)
        {
            builder.HasKey(sp => sp.SalesPaymentId);
            builder.Property(sp => sp.PaymentDate);
            builder.Property(sp => sp.AmountPaid)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(sp => sp.DueDate)
                   .IsRequired();
            builder.Property(sp => sp.PaymentMode)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(sp => sp.PaymentStatus)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.HasOne<Sale>()
                   .WithMany()
                   .HasForeignKey(sp => sp.SaleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
