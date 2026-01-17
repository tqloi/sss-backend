using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Planning;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Planning;

public class StudyPlanModuleConfiguration : IEntityTypeConfiguration<StudyPlanModule>
{
    public void Configure(EntityTypeBuilder<StudyPlanModule> builder)
    {
        builder.ToTable("StudyPlanModules");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.StudyPlanId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.RoadmapNodeId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.Status)
            .HasMaxLength(20);

        builder.HasOne(e => e.StudyPlan)
            .WithMany(p => p.Modules)
            .HasForeignKey(e => e.StudyPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.RoadmapNode)
            .WithMany(n => n.StudyPlanModules)
            .HasForeignKey(e => e.RoadmapNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.StudyPlanId, e.RoadmapNodeId }).IsUnique();
    }
}