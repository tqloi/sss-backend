using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.ToTable("QuizAttempts");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.QuizId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(e => e.StartedAt)
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(e => e.SubmittedAt)
            .HasColumnType("datetime(6)");

        builder.Property(e => e.Score)
            .HasColumnType("decimal(6,2)");

        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(e => e.Quiz)
            .WithMany(q => q.Attempts)
            .HasForeignKey(e => e.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Answers)
            .WithOne(a => a.Attempt)
            .HasForeignKey(a => a.AttemptId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}