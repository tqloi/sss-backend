using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Sercurity.Jwt;
using SSS.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SSS.Infrastructure.Sercurity.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAppDbContext _db;
        private readonly JwtOptions _options;

        public JwtTokenService(
            UserManager<User> userManager,
            IOptions<JwtOptions> settings,
            IAppDbContext db)
        {
            _userManager = userManager;
            _db = db;
            _options = settings.Value;
        }

        // ===== Public API =====

        /// <summary>
        /// Issue access + refresh token pair
        /// </summary>
        //public async Task<AuthResult> IssueAsync(User user, string? clientIp, CancellationToken ct = default)
        //{
        //    // 1) Access token
        //    var (accessToken, accessExp) = await GenerateAccessAsync(user);

        //    // 2) Refresh token
        //    var refreshPlain = RefreshTokenUtil.GeneratePlaintext();
        //    var refreshHash = RefreshTokenUtil.Hash(refreshPlain);
        //    var refreshExp = DateTime.Now.AddDays(_options.ExpireDays);

        //    var rt = new RefreshToken
        //    {
        //        UserId = user.Id,
        //        TokenHash = refreshHash,
        //        ExpiresAtUtc = refreshExp,
        //        CreatedByIp = clientIp
        //    };

        //    _db.Set<RefreshToken>().Add(rt);
        //    await _db.SaveChangesAsync(ct);

        //    // 3) Build ticket
        //    //return await BuildTicketAsync(user, accessToken, accessExp, refreshPlain, refreshExp, ct);
        //}

        /// <summary>
        /// Generate JWT access token
        /// </summary>
        public async Task<(string token, DateTime expiresUtc)> GenerateAccessAsync(User user)
        {
            var now = DateTime.Now;
            var expires = now.AddMinutes(_options.ExpireMinutes);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new("sub",  user.Id),
                new("name", user.UserName ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };
            // ClaimTypes.Role để [Authorize(Roles="...")] hoạt động chuẩn
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expires);
        }

        ///// <summary>
        ///// Build AuthResult (ticket)
        ///// </summary>
        //public async Task<AuthResult> BuildTicketAsync(
        //    User user,
        //    string accessToken,
        //    DateTime accessExp,
        //    string refreshPlain,
        //    DateTime refreshExp,
        //    CancellationToken ct)
        //{
        //    var roles = await _userManager.GetRolesAsync(user);

        //    return new AuthResult
        //    {
        //        AccessToken = accessToken,
        //        AccessTokenExpiresUtc = accessExp,
        //        RefreshTokenPlain = refreshPlain,
        //        RefreshTokenExpiresUtc = refreshExp,
        //        User = new UserDto
        //        {
        //            Id = user.Id,
        //            Email = user.Email ?? string.Empty,
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            AvatarUrl = user.ProfileImagePath,
        //            Roles = roles.ToList()
        //        }
        //    };
        //}
    }
}
