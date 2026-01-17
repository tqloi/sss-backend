using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment;

public class QuizAnswerConfiguration : IEntityTypeConfiguration<QuizAnswer>
{
    public void Configure(EntityTypeBuilder<QuizAnswer> builder)
    {
        builder.ToTable("QuizAnswers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.AttemptId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.QuestionId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.OptionId)
            .HasColumnType("bigint");

        builder.Property(e => e.TextValue)
            .HasColumnType("text");

        builder.Property(e => e.NumberValue)
            .HasColumnType("decimal(10,4)");

        builder.Property(e => e.ScoredValue)
            .HasColumnType("decimal(6,2)");

        builder.Property(e => e.AnsweredAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.HasOne(e => e.Attempt)
            .WithMany(a => a.Answers)
            .HasForeignKey(e => e.AttemptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Option)
            .WithMany(o => o.Answers)
            .HasForeignKey(e => e.OptionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => new { e.AttemptId, e.QuestionId }).IsUnique();
    }
}