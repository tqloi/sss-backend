using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Learning;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Learning
{
    public class UserLearningBehaviorConfiguration
    : IEntityTypeConfiguration<UserLearningBehavior>
    {
        public void Configure(EntityTypeBuilder<UserLearningBehavior> builder)
        {
            builder.ToTable("UserLearningBehaviors");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

            builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(255);

            builder.HasIndex(x => x.UserId)
            .IsUnique();

            // JSON columns (MySQL native)
            builder.Property(x => x.AvailableDaysJson)
            .HasColumnType("json");

            builder.Property(x => x.PreferredTimeBlocksJson)
            .HasColumnType("json");

            builder.Property(x => x.CommonDifficultiesJson)
            .HasColumnType("json");

            // DECIMAL for AI weights
            builder.Property(x => x.WVisual)
            .HasPrecision(6, 4);

            builder.Property(x => x.WReading)
            .HasPrecision(6, 4);

            builder.Property(x => x.WPractice)
            .HasPrecision(6, 4);

            builder.Property(x => x.DisciplineType)
            .HasMaxLength(50);

            // MySQL datetime with microseconds
            builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime(6)");
        }
    }
}
