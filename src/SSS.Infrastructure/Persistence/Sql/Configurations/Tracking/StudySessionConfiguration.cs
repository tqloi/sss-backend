using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Tracking;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Tracking;

public class StudySessionConfiguration : IEntityTypeConfiguration<StudySession>
{
    public void Configure(EntityTypeBuilder<StudySession> builder)
    {
        builder.ToTable("StudySessions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("char(24)")
            .HasMaxLength(24)
            .IsFixedLength()
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();



        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.StartAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(e => e.EndAt)
            .HasColumnType("datetime(6)");

        builder.Property(e => e.PausedAt)
            .HasColumnType("datetime(6)");

        builder.Property(e => e.EndedReason)
            .HasMaxLength(30);

        builder.Property(e => e.TasksCompletedCount)
            .HasColumnType("int");

        builder.Property(e => e.TotalTasks)
            .HasColumnType("int");

        builder.Property(e => e.PlannedDurationSeconds)
            .HasColumnType("int");

        builder.Property(e => e.ActualDurationSeconds)
            .HasColumnType("int");

        builder.Property(e => e.ActiveSeconds)
            .HasColumnType("int");

        builder.Property(e => e.IdleSeconds)
            .HasColumnType("int");

        builder.Property(e => e.PauseCount)
            .HasColumnType("int");

        builder.Property(e => e.PauseSeconds)
            .HasColumnType("int");

        builder.Property(e => e.FocusScore)
            .HasColumnType("int");

        builder.Property(e => e.ConfidenceActiveLearning)
            .HasColumnType("decimal(5,4)");

        builder.Property(e => e.FatigueScore)
            .HasColumnType("int");

        builder.Property(e => e.SelfRating)
            .HasColumnType("int");

        builder.Property(e => e.LocalTimeBlock)
            .HasMaxLength(20);

        builder.Property(e => e.Timezone)
            .HasMaxLength(50);

        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime(6)");

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.UserId, e.StartAt });
    }
}