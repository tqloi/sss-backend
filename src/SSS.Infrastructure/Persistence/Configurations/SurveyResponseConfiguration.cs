using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class SurveyResponseConfiguration : IEntityTypeConfiguration<SurveyResponse>
{
    public void Configure(EntityTypeBuilder<SurveyResponse> builder)
    {
        builder.ToTable("SurveyResponses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.SurveyId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(e => e.TriggerReason)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(e => e.StartedAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(e => e.SubmittedAt)
            .HasColumnType("datetime(6)");

        builder.Property(e => e.SnapshotJson)
            .HasColumnType("json");

        builder.HasOne(e => e.Survey)
            .WithMany(s => s.Responses)
            .HasForeignKey(e => e.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Answers)
            .WithOne(a => a.Response)
            .HasForeignKey(a => a.ResponseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}