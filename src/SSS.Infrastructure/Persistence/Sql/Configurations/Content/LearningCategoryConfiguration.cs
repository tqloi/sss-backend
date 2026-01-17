using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Content;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Content;

public class LearningCategoryConfiguration : IEntityTypeConfiguration<LearningCategory>
{
    public void Configure(EntityTypeBuilder<LearningCategory> builder)
    {
        builder.ToTable("LearningCategories");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnType("text");

        builder.Property(e => e.IsActive)
            .HasColumnType("tinyint(1)")
            .IsRequired();
    }
}