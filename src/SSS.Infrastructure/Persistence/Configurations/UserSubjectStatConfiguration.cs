using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class UserSubjectStatConfiguration : IEntityTypeConfiguration<UserSubjectStat>
{
    public void Configure(EntityTypeBuilder<UserSubjectStat> builder)
    {
        builder.ToTable("UserSubjectStats");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(e => e.SubjectId)
            .HasColumnType("bigint")
            .IsRequired();

        builder.Property(e => e.ProficiencyLevel)
            .HasColumnType("decimal(5,2)");

        builder.Property(e => e.TotalHoursSpent)
            .HasColumnType("decimal(10,2)");

        builder.Property(e => e.WeakNodeIds)
            .HasColumnType("json");

        builder.Property(e => e.UpdatedAt)
            .HasColumnType("datetime(6)");

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Subject)
            .WithMany()
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.UserId, e.SubjectId }).IsUnique();
    }
}