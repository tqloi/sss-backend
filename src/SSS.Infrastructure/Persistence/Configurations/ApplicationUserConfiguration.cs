using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities;

namespace SSS.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("AspNetUsers");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(u => u.UserName)
            .HasMaxLength(256);

        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(256);

        builder.HasIndex(u => u.NormalizedUserName)
            .IsUnique();

        builder.Property(u => u.Email)
            .HasMaxLength(256);

        builder.Property(u => u.NormalizedEmail)
            .HasMaxLength(256);

        builder.Property(u => u.EmailConfirmed)
            .HasColumnType("tinyint(1)");

        builder.Property(u => u.PasswordHash)
            .HasColumnType("text");

        builder.Property(u => u.SecurityStamp)
            .HasColumnType("text");

        builder.Property(u => u.ConcurrencyStamp)
            .HasColumnType("text");

        builder.Property(u => u.PhoneNumber)
            .HasColumnType("text");

        builder.Property(u => u.PhoneNumberConfirmed)
            .HasColumnType("tinyint(1)");

        builder.Property(u => u.TwoFactorEnabled)
            .HasColumnType("tinyint(1)");

        builder.Property(u => u.LockoutEnd)
            .HasColumnType("datetime(6)");

        builder.Property(u => u.LockoutEnabled)
            .HasColumnType("tinyint(1)");

        builder.Property(u => u.AccessFailedCount)
            .HasColumnType("int");

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}