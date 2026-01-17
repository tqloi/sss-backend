using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment;

public class SurveyAnswerConfiguration : IEntityTypeConfiguration<SurveyAnswer>
{
    public void Configure(EntityTypeBuilder<SurveyAnswer> builder)
    {
        builder.ToTable("SurveyAnswers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.ResponseId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.QuestionId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.OptionId)
            .HasColumnType("bigint");

        builder.Property(e => e.NumberValue)
            .HasColumnType("decimal(10,4)");

        builder.Property(e => e.TextValue)
            .HasColumnType("text");

        builder.Property(e => e.AnsweredAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.HasOne(e => e.Response)
            .WithMany(r => r.Answers)
            .HasForeignKey(e => e.ResponseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Option)
            .WithMany(o => o.Answers)
            .HasForeignKey(e => e.OptionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}