using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Content;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Content;

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

        builder.HasIndex(n => new { n.RoadmapId, n.OrderNo });
    }
}