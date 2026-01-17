using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.StudyPlanModuleId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.Title)
            .HasMaxLength(300);

        builder.Property(e => e.Description)
            .HasColumnType("text");

        builder.Property(e => e.TotalScore)
            .HasColumnType("decimal(6,2)");

        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.HasOne(e => e.StudyPlanModule)
            .WithMany(m => m.Quizzes)
            .HasForeignKey(e => e.StudyPlanModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.StudyPlanModuleId).IsUnique();
    }
}