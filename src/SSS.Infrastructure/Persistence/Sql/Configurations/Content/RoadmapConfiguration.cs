using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Content;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Content;

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

        builder.Property(e => e.Version)
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(e => e.IsLatest)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(e => e.CreatedAt);

        builder.Property(e => e.CreateById)
            .HasMaxLength(450);

        builder.HasOne(e => e.CreateBy)
            .WithMany()
            .HasForeignKey(e => e.CreateById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.SubjectId, e.Title, e.Version })
            .IsUnique();

        builder.HasIndex(e => new { e.SubjectId, e.Title, e.IsLatest });
    }
}