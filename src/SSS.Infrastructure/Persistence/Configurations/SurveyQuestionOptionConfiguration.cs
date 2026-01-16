using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class SurveyQuestionOptionConfiguration : IEntityTypeConfiguration<SurveyQuestionOption>
{
    public void Configure(EntityTypeBuilder<SurveyQuestionOption> builder)
    {
        builder.ToTable("SurveyQuestionOptions");

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

        builder.Property(e => e.Weight)
            .HasColumnType("decimal(6,4)");

        builder.Property(e => e.OrderNo)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(e => e.AllowFreeText)
            .HasColumnType("tinyint(1)")
            .IsRequired();

        builder.HasOne(e => e.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Answers)
            .WithOne(a => a.Option)
            .HasForeignKey(a => a.OptionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => new { e.QuestionId, e.ValueKey }).IsUnique();
    }
}