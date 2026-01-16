using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class UserBehaviorWindowConfiguration : IEntityTypeConfiguration<UserBehaviorWindow>
{
    public void Configure(EntityTypeBuilder<UserBehaviorWindow> builder)
    {
        builder.ToTable("UserBehaviorWindows");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(e => e.WindowStart)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(e => e.WindowEnd)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(e => e.AvgFocusScore)
            .HasColumnType("decimal(5,2)");

        builder.Property(e => e.ActiveRatio)
            .HasColumnType("decimal(5,4)");

        builder.Property(e => e.AvgSessionLengthMinutes)
            .HasColumnType("int");

        builder.Property(e => e.CompletionRate)
            .HasColumnType("decimal(5,4)");

        builder.Property(e => e.ComputedAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.UserId, e.WindowStart, e.WindowEnd }).IsUnique();
    }
}