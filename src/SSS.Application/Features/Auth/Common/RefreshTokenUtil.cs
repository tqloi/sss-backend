using System.Security.Cryptography;
using System.Text;

namespace SSS.Application.Features.Auth.Common;

public static class RefreshTokenUtil
{
    public static string GeneratePlaintext()
    {
        var bytes = RandomNumberGenerator.GetBytes(32); // 256-bit
        return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_'); // Base64Url
    }

    public static string Hash(string tokenPlaintext)
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(tokenPlaintext));
        return Convert.ToHexString(hash);
    }
}
