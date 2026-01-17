using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment;

public class QuizQuestionOptionConfiguration : IEntityTypeConfiguration<QuizQuestionOption>
{
    public void Configure(EntityTypeBuilder<QuizQuestionOption> builder)
    {
        builder.ToTable("QuizQuestionOptions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.QuestionId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.ValueKey)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.DisplayText)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.IsCorrect)
            .HasColumnType("tinyint(1)")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.ScoreValue)
            .HasColumnType("decimal(5,2)");

        builder.Property(e => e.OrderNo)
            .HasColumnType("int")
            .IsRequired();

        builder.HasOne(e => e.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(e => new { e.QuestionId, e.ValueKey }).IsUnique();
    }
}