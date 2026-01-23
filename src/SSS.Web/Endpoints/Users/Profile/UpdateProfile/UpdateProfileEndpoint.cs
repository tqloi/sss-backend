using FastEndpoints;
using MediatR;
using SSS.Application.Features.Users.Profile.UpdateProfile;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Users.Profile.UpdateProfile;

public sealed class UpdateProfileEndpoint : Endpoint<UpdateProfileRequest, UpdateProfileResponse>
{
    private readonly ISender _sender;
    private readonly AutoMapper.IMapper _mapper;

    public UpdateProfileEndpoint(ISender sender, AutoMapper.IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/api/users/profile");
        Description(d => d.WithTags("Users"));
    }

    public override async Task HandleAsync(UpdateProfileRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var command = _mapper.Map<UpdateProfileCommand>(req);
        command.UserId = userId;

        var result = await _sender.Send(command, ct);
        var response = _mapper.Map<UpdateProfileResponse>(result);

        await SendOkAsync(response, ct);
    }
}
