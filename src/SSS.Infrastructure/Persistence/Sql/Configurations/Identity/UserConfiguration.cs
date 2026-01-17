using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Identity;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(e => e.Id);

        builder.Property(x => x.AvatarUrl).HasColumnType("varchar(512)");
        builder.Property(x => x.Address).HasColumnType("longtext");
        builder.Property(x => x.Dob).HasColumnType("date");
    }
}