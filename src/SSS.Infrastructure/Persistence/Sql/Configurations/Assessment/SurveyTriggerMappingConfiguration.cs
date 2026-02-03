using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment
{
    public class SurveyTriggerMappingConfiguration
           : IEntityTypeConfiguration<SurveyTriggerMapping>
    {
        public void Configure(EntityTypeBuilder<SurveyTriggerMapping> builder)
        {
            builder.ToTable("SurveyTriggerMappings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TriggerType)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.MaxAttempts);

            builder.Property(x => x.CooldownDays);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt);

            builder.HasOne(x => x.Survey)
                   .WithMany(s => s.TriggerMappings)
                   .HasForeignKey(x => x.SurveyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.SurveyId, x.TriggerType })
                   .IsUnique();
        }
    }
}
