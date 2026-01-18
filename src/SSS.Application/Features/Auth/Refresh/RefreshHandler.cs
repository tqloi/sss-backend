using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Sercurity.Jwt;
using SSS.Application.Features.Auth.Common;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.Auth.Refresh
{
    public sealed class RefreshHandler : IRequestHandler<RefreshCommand, AuthResult>
    {
        private readonly IAppDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwt;
        private readonly IConfiguration _config;

        public RefreshHandler(IAppDbContext db,
                              UserManager<User> userManager,
                              IJwtTokenService jwt,
                              IConfiguration config)
        {
            _db = db;
            _userManager = userManager;
            _jwt = jwt;
            _config = config;
        }

        public async Task<AuthResult> Handle(RefreshCommand req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.RefreshTokenPlain))
                throw new UnauthorizedAccessException("Missing refresh token");

            var refreshHash = RefreshTokenUtil.Hash(req.RefreshTokenPlain);

            var rt = await _db.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == refreshHash, ct);

            if (rt == null || rt.RevokedAtUtc != null || rt.ExpiresAtUtc <= DateTime.Now || rt.IsUsed)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var user = await _userManager.FindByIdAsync(rt.UserId);
            if (user is null)
                throw new UnauthorizedAccessException("User not found");

            // rotate: revoke token cũ + phát hành token mới
            rt.IsUsed = true;
            rt.RevokedAtUtc = DateTime.Now;
            rt.RevokedByIp = req.RequestIp;

            var expiresUtc = DateTime.Now.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"] ?? "60"));
            var newPlain = RefreshTokenUtil.GeneratePlaintext();
            var newHash = RefreshTokenUtil.Hash(newPlain);

            var newRt = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = newHash,
                ExpiresAtUtc = DateTime.Now.AddDays(int.Parse(_config["Jwt:ExpireDays"] ?? "7")),
                CreatedByIp = req.RequestIp
            };

            _db.RefreshTokens.Add(newRt);
            await _db.SaveChangesAsync(ct);

            // nếu schema dùng ReplacedByTokenId
            rt.ReplacedByTokenId = newRt.Id;
            await _db.SaveChangesAsync(ct);

            // Create new access token
            var (access, accessExp) = await _jwt.GenerateAccessAsync(user);

            // Build ticket
            return await _jwt.BuildTicketAsync(user, access, accessExp, newPlain, newRt.ExpiresAtUtc, ct);
        }
    }
}