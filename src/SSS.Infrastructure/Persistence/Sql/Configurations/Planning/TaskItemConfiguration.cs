using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Planning;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Planning;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("TaskItems");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.StudyPlanModuleId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.Title)
            .HasMaxLength(400)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasMaxLength(20);

        builder.Property(e => e.ScheduledDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.CompletedAt)
            .HasColumnType("datetime(6)");

        builder.HasOne(e => e.StudyPlanModule)
            .WithMany(m => m.Tasks)
            .HasForeignKey(e => e.StudyPlanModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => new { t.StudyPlanModuleId, t.ScheduledDate });
        builder.HasIndex(t => new { t.StudyPlanModuleId, t.Status, t.ScheduledDate });
    }
}