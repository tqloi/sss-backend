using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class RoadmapConfiguration : IEntityTypeConfiguration<Roadmap>
{
    public void Configure(EntityTypeBuilder<Roadmap> builder)
    {
        builder.ToTable("Roadmaps");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.SubjectId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnType("text");

        builder.HasOne(e => e.Subject)
            .WithMany(s => s.Roadmaps)
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Nodes)
            .WithOne(n => n.Roadmap)
            .HasForeignKey(n => n.RoadmapId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Edges)
            .WithOne(e => e.Roadmap)
            .HasForeignKey(e => e.RoadmapId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}