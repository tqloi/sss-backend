namespace SSS.Domain.Entities.Identity;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string TokenHash { get; set; } = null!;
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string? CreatedByIp { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? RevokedByIp { get; set; }
    public Guid? ReplacedByTokenId { get; set; }
    public bool IsUsed { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public RefreshToken? ReplacedByToken { get; set; }
    public ICollection<RefreshToken> ReplacedTokens { get; set; } = new HashSet<RefreshToken>();
}