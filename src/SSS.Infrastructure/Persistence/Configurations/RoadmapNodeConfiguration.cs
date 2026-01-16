using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class RoadmapNodeConfiguration : IEntityTypeConfiguration<RoadmapNode>
{
    public void Configure(EntityTypeBuilder<RoadmapNode> builder)
    {
        builder.ToTable("RoadmapNodes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.RoadmapId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnType("text");

        builder.Property(e => e.Difficulty)
            .HasMaxLength(20);

        builder.Property(e => e.OrderNo)
            .HasColumnType("int");

        builder.HasOne(e => e.Roadmap)
            .WithMany(r => r.Nodes)
            .HasForeignKey(e => e.RoadmapId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Contents)
            .WithOne(c => c.Node)
            .HasForeignKey(c => c.NodeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.OutgoingEdges)
            .WithOne(e => e.FromNode)
            .HasForeignKey(e => e.FromNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.IncomingEdges)
            .WithOne(e => e.ToNode)
            .HasForeignKey(e => e.ToNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.StudyPlanModules)
            .WithOne(m => m.RoadmapNode)
            .HasForeignKey(m => m.RoadmapNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.StudySessions)
            .WithOne(s => s.Node)
            .HasForeignKey(s => s.NodeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}