using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class RoadmapEdgeConfiguration : IEntityTypeConfiguration<RoadmapEdge>
{
    public void Configure(EntityTypeBuilder<RoadmapEdge> builder)
    {
        builder.ToTable("RoadmapEdges");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.RoadmapId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.FromNodeId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.ToNodeId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.EdgeType)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(e => e.OrderNo)
            .HasColumnType("int");

        builder.HasOne(e => e.Roadmap)
            .WithMany(r => r.Edges)
            .HasForeignKey(e => e.RoadmapId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.FromNode)
            .WithMany(n => n.OutgoingEdges)
            .HasForeignKey(e => e.FromNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ToNode)
            .WithMany(n => n.IncomingEdges)
            .HasForeignKey(e => e.ToNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.RoadmapId, e.FromNodeId, e.ToNodeId, e.EdgeType })
            .IsUnique();
    }
}