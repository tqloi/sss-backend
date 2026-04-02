using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Payment;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Payment
{
    public class UserPaymentConfiguration : IEntityTypeConfiguration<UserPayment>
    {
        public void Configure(EntityTypeBuilder<UserPayment> builder)
        {
            builder.ToTable("UserPayments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("VND")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>() // enum -> NVARCHAR
                .HasMaxLength(20)
                .HasDefaultValue(PaymentStatus.Pending)
                .IsRequired();

            builder.Property(x => x.PaymentDate)
                .IsRequired();

            // Index hay dùng
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.PaymentDate);
        }
    }
}
