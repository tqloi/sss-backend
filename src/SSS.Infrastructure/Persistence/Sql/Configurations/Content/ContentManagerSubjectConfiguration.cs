using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Content;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Content
{
    public class ContentManagerSubjectConfiguration : IEntityTypeConfiguration<ContentManagerSubject>
    {
        public void Configure(EntityTypeBuilder<ContentManagerSubject> builder)
        {
            builder.ToTable("ContentManagerSubject");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ContentManagerId)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.AssignedBy)
                   .HasMaxLength(255);

            builder.Property(x => x.AssignedAt)
                   .HasColumnType("datetime");

            builder.Property(x => x.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            // Unique constraint: one manager – one subject
            builder.HasIndex(x => new { x.ContentManagerId, x.SubjectId })
                   .IsUnique();

            // Indexes
            builder.HasIndex(x => x.ContentManagerId);
            builder.HasIndex(x => x.SubjectId);

            // Relationships
            builder.HasOne(x => x.ContentManager)
                   .WithMany()
                   .HasForeignKey(x => x.ContentManagerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssignedByUser)
                   .WithMany()
                   .HasForeignKey(x => x.AssignedBy)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Subject)
                   .WithMany()
                   .HasForeignKey(x => x.SubjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
