using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Content;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Content
{
    public class ReviewEntityConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
                 builder.ToTable("Reviews");
                 builder.HasKey(x => x.Id);
                 builder.Property(x => x.RoadmapId).IsRequired();
                 builder.Property(x => x.ReviewerId).HasMaxLength(255);
                 builder.Property(x => x.Comment).HasMaxLength(2000);
                 builder.Property(x => x.Rating).IsRequired();
                 builder.Property(x => x.CreatedAt).IsRequired();
                 builder.Property(x => x.UpdatedAt);

                 builder.HasOne(x => x.Roadmap)
                     .WithMany()
                     .HasForeignKey(x => x.RoadmapId)
                     .OnDelete(DeleteBehavior.Cascade);

                 builder.HasOne(x => x.Reviewer)
                     .WithMany()
                     .HasForeignKey(x => x.ReviewerId)
                     .OnDelete(DeleteBehavior.SetNull);

                builder.HasIndex(r => new { r.RoadmapId, r.ReviewerId }).IsUnique();
        }
    }
}