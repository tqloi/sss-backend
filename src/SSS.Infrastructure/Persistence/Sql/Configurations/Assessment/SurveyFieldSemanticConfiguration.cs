using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment
{
    public class SurveyFieldSemanticConfiguration
           : IEntityTypeConfiguration<SurveyFieldSemantic>
    {
        public void Configure(EntityTypeBuilder<SurveyFieldSemantic> builder)
        {
            builder.ToTable("SurveyFieldSemantics");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DimensionCode)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Evaluates)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.AIHint)
                   .HasMaxLength(1000);

            builder.Property(x => x.Weight);

            builder.Property(x => x.CreatedAt);

            builder.HasOne(x => x.SurveyQuestion)
                   .WithMany(q => q.Semantics)
                   .HasForeignKey(x => x.SurveyQuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
