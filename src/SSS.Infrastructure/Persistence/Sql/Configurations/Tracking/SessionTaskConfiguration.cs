using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Tracking;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Tracking;

public class SessionTaskConfiguration : IEntityTypeConfiguration<SessionTask>
{
    public void Configure(EntityTypeBuilder<SessionTask> builder)
    {
        builder.ToTable("SessionTasks");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.StudySessionId)
            .HasColumnType("char(24)")
            .HasMaxLength(24)
            .IsFixedLength()
            .IsRequired();

        builder.Property(e => e.TaskId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.StartTimeUtc)
            .HasColumnType("datetime(6)");

        builder.Property(e => e.EndTimeUtc)
            .HasColumnType("datetime(6)");

        builder.HasOne(e => e.StudySession)
            .WithMany(s => s.SessionTasks)
            .HasForeignKey(e => e.StudySessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.TaskItem)
            .WithMany(t => t.SessionTasks)
            .HasForeignKey(e => e.TaskId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasIndex(e => e.StudySessionId);
        builder.HasIndex(e => e.TaskId);
    }
}
