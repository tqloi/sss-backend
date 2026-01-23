using MediatR;

namespace SSS.Application.Features.Users.Profile.UploadAvatar;

public sealed class UploadAvatarCommand : IRequest<UploadAvatarResult>
{
    public string UserId { get; set; } = default!;
    public Stream FileStream { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
}
