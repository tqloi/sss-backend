using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSS.Domain.Entities.Identity;

namespace SSS.Infrastructure.Persistence.Sql.Configurations.Identity;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .IsRequired();

        builder.Property(t => t.UserId)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(t => t.TokenHash)
            .HasColumnType("char(64)")
            .IsRequired();

        builder.HasIndex(t => t.TokenHash)
            .IsUnique();

        builder.Property(t => t.ExpiresAtUtc)
            .HasColumnType("datetime(6)");

        builder.Property(t => t.CreatedAtUtc)
            .HasColumnType("datetime(6)");

        builder.Property(t => t.CreatedByIp)
            .HasMaxLength(45);

        builder.Property(t => t.RevokedAtUtc)
            .HasColumnType("datetime(6)");

        builder.Property(t => t.RevokedByIp)
            .HasMaxLength(45);

        //builder.Property(t => t.ReplacedByTokenId)
        //    .HasColumnType("char(36)");

        builder.Property(t => t.IsUsed)
            .HasColumnType("tinyint(1)");

        builder.HasOne(t => t.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.ReplacedByToken)
            .WithMany(t => t.ReplacedTokens)
            .HasForeignKey(t => t.ReplacedByTokenId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}