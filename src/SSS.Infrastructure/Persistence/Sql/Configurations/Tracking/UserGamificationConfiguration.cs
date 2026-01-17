using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Tracking;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Tracking;

public class UserGamificationConfiguration : IEntityTypeConfiguration<UserGamification>
{
    public void Configure(EntityTypeBuilder<UserGamification> builder)
    {
        builder.ToTable("UserGamification");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(e => e.CurrentStreak)
            .HasColumnType("int");

        builder.Property(e => e.LongestStreak)
            .HasColumnType("int");

        builder.Property(e => e.LastActiveDate)
            .HasColumnType("date");

        builder.Property(e => e.TotalExp)
            .HasColumnType("int");

        builder.Property(e => e.UpdatedAt)
            .HasColumnType("datetime(6)");

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.UserId).IsUnique();
    }
}