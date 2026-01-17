using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment;

public class SurveyQuestionConfiguration : IEntityTypeConfiguration<SurveyQuestion>
{
    public void Configure(EntityTypeBuilder<SurveyQuestion> builder)
    {
        builder.ToTable("SurveyQuestions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.SurveyId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.QuestionKey)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Prompt)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(e => e.Type)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(e => e.ScaleMin)
            .HasColumnType("int");

        builder.Property(e => e.ScaleMax)
            .HasColumnType("int");

        builder.Property(e => e.OrderNo)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.IsRequired)
            .HasColumnType("tinyint(1)")
            .IsRequired();

        builder.HasOne(e => e.Survey)
            .WithMany(s => s.Questions)
            .HasForeignKey(e => e.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}