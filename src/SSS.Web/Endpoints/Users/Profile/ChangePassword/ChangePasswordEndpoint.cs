using FastEndpoints;
using MediatR;
using SSS.Application.Features.Users.Profile.ChangePassword;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Users.Profile.ChangePassword;

public sealed class ChangePasswordEndpoint : Endpoint<ChangePasswordRequest, ChangePasswordResponse>
{
    private readonly ISender _sender;
    private readonly AutoMapper.IMapper _mapper;

    public ChangePasswordEndpoint(ISender sender, AutoMapper.IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/api/users/password");
        Description(d => d.WithTags("Users"));
    }

    public override async Task HandleAsync(ChangePasswordRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var command = new ChangePasswordCommand
        {
            UserId = userId,
            CurrentPassword = req.CurrentPassword,
            NewPassword = req.NewPassword
        };

        var result = await _sender.Send(command, ct);
        var response = _mapper.Map<ChangePasswordResponse>(result);

        await SendOkAsync(response, ct);
    }
}
