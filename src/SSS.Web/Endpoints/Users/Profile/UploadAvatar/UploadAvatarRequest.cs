using Microsoft.AspNetCore.Http;

namespace SSS.WebApi.Endpoints.Users.Profile.UploadAvatar;

public sealed class UploadAvatarRequest
{
    public IFormFile Avatar { get; set; } = default!;
}
