using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Assessment;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Assessment;

public class SurveyTriggerTypeConfiguration : IEntityTypeConfiguration<SurveyTriggerType>
{
    public void Configure(EntityTypeBuilder<SurveyTriggerType> builder)
    {
        builder.ToTable("SurveyTriggerTypes");

        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.DisplayName)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(500);

        builder.Property(x => x.IsActive)
               .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt);
   
    }
}