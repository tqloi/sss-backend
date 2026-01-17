using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Planning;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Planning;

public class StudyPlanConfiguration : IEntityTypeConfiguration<StudyPlan>
{
    public void Configure(EntityTypeBuilder<StudyPlan> builder)
    {
        builder.ToTable("StudyPlans");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(e => e.RoadmapId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.ProfileVersion)
            .HasColumnType("int");

        builder.Property(e => e.Status)
            .HasMaxLength(20);

        builder.Property(e => e.Strategy)
            .HasMaxLength(20);

        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Roadmap)
            .WithMany(r => r.StudyPlans)
            .HasForeignKey(e => e.RoadmapId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}