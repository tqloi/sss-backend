using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Content;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Content;

public class LearningSubjectConfiguration : IEntityTypeConfiguration<LearningSubject>
{
    public void Configure(EntityTypeBuilder<LearningSubject> builder)
    {
        builder.ToTable("LearningSubjects");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CategoryId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnType("text");

        builder.Property(e => e.IsActive)
            .HasColumnType("tinyint(1)")
            .IsRequired();

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Subjects)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}