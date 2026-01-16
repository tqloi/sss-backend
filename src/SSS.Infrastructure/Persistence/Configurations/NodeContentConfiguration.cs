using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class NodeContentConfiguration : IEntityTypeConfiguration<NodeContent>
{
    public void Configure(EntityTypeBuilder<NodeContent> builder)
    {
        builder.ToTable("NodeContents");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.NodeId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.ContentType)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(e => e.Title)
            .HasMaxLength(400)
            .IsRequired();

        builder.Property(e => e.Url)
            .HasMaxLength(2048);

        builder.Property(e => e.Description)
            .HasColumnType("text");

        builder.Property(e => e.EstimatedMinutes)
            .HasColumnType("int");

        builder.Property(e => e.Difficulty)
            .HasMaxLength(20);

        builder.Property(e => e.OrderNo)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.IsRequired)
            .HasColumnType("tinyint(1)")
            .IsRequired();

        builder.HasOne(e => e.Node)
            .WithMany(n => n.Contents)
            .HasForeignKey(e => e.NodeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}   