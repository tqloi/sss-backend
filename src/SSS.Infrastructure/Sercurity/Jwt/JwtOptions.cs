using System.ComponentModel.DataAnnotations;

namespace SSS.Infrastructure.Sercurity.Jwt
{
    public class JwtOptions
    {
        [Required, MinLength(32, ErrorMessage = "Secret must be at least 32 chars.")]
        public string Key { get; set; } = default!;

        [Required] public string Issuer { get; set; } = default!;
        [Required] public string Audience { get; set; } = default!;

        [Range(1, 24 * 60)]
        public int ExpireMinutes { get; set; } = 60;

        [Range(1, 365)]
        public int ExpireDays { get; set; } = 7;
    }
}
