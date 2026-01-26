using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Entities.Learning;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment
{
    public class UserLearningTargetConfiguration
: IEntityTypeConfiguration<UserLearningTarget>
    {
        public void Configure(EntityTypeBuilder<UserLearningTarget> builder)
        {
            builder.ToTable("UserLearningTargets");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

            builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(255);

            builder.HasIndex(x => x.UserId);

            builder.Property(x => x.ProfileVersion)
            .IsRequired();

            builder.Property(x => x.TargetRole)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(x => x.CurrentLevel)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(x => x.TargetDeadlineMonths);

            builder.Property(x => x.GoalDescription)
            .HasColumnType("text");

            builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime(6)");

            builder.HasIndex(x => new { x.UserId, x.RoadmapId, x.Status });
        }
    }
}
