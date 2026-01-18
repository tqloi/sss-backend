using SSS.Application.Features.Auth.Common;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Abstractions.Sercurity.Jwt
{
    public interface IJwtTokenService
    {
        Task<AuthResult> IssueAsync(User user, string? clientIp, CancellationToken ct = default);

        Task<AuthResult> BuildTicketAsync(
            User user,
            string accessToken,
            DateTime accessExp,
            string refreshPlain,
            DateTime refreshExp,
            CancellationToken ct);

        Task<(string token, DateTime expiresUtc)> GenerateAccessAsync(User user);
    }
}
