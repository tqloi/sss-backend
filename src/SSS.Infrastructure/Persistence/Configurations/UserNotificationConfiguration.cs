using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        builder.ToTable("UserNotifications");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(e => e.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.Content)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(e => e.Type)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(e => e.RelatedType)
            .HasMaxLength(30);

        builder.Property(e => e.RelatedId)
            .HasColumnType("bigint");

        builder.Property(e => e.RelatedSessionId)
            .HasColumnType("char(24)")
            .HasMaxLength(24)
            .IsFixedLength();

        builder.Property(e => e.IsRead)
            .HasColumnType("tinyint(1)")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.ReadAt)
            .HasColumnType("datetime(6)");

        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.RelatedSession)
            .WithMany()
            .HasForeignKey(e => e.RelatedSessionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => new { e.UserId, e.CreatedAt });
        builder.HasIndex(e => new { e.UserId, e.IsRead, e.CreatedAt });
    }
}