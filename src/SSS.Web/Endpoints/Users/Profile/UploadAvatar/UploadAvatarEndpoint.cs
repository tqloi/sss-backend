using FastEndpoints;
using MediatR;
using SSS.Application.Features.Users.Profile.UploadAvatar;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Users.Profile.UploadAvatar;

public sealed class UploadAvatarEndpoint : Endpoint<UploadAvatarRequest, UploadAvatarResponse>
{
    private readonly ISender _sender;
    private readonly AutoMapper.IMapper _mapper;

    public UploadAvatarEndpoint(ISender sender, AutoMapper.IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post("/api/users/avatar");
        AllowFileUploads();
        Description(d => d
            .WithTags("Users")
            .Accepts<UploadAvatarRequest>("multipart/form-data"));
    }

    public override async Task HandleAsync(UploadAvatarRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        if (req.Avatar is null || req.Avatar.Length == 0)
        {
            AddError("No file uploaded");
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var command = new UploadAvatarCommand
        {
            UserId = userId,
            FileStream = req.Avatar.OpenReadStream(),
            FileName = req.Avatar.FileName,
            ContentType = req.Avatar.ContentType
        };

        var result = await _sender.Send(command, ct);
        var response = _mapper.Map<UploadAvatarResponse>(result);

        await SendOkAsync(response, ct);
    }
}
