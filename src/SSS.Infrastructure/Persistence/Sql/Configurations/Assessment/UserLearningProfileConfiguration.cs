using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment;

public class UserLearningProfileConfiguration : IEntityTypeConfiguration<UserLearningProfile>
{
    public void Configure(EntityTypeBuilder<UserLearningProfile> builder)
    {
        builder.ToTable("UserLearningProfiles");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(e => e.ProfileVersion)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.TargetRole)
            .HasMaxLength(50);

        builder.Property(e => e.CurrentLevel)
            .HasMaxLength(50);

        builder.Property(e => e.TargetDeadlineMonths)
            .HasColumnType("int");

        builder.Property(e => e.AvailableDaysJson)
            .HasColumnType("json");

        builder.Property(e => e.PreferredTimeBlocksJson)
            .HasColumnType("json");

        builder.Property(e => e.SessionLengthPrefMinutes)
            .HasColumnType("int");

        builder.Property(e => e.WVisual)
            .HasColumnType("decimal(6,4)");

        builder.Property(e => e.WReading)
            .HasColumnType("decimal(6,4)");

        builder.Property(e => e.WPractice)
            .HasColumnType("decimal(6,4)");

        builder.Property(e => e.ConfOverall)
            .HasColumnType("decimal(5,4)");

        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.UserId, e.ProfileVersion }).IsUnique();
    }
}