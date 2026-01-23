using FastEndpoints;
using MediatR;
using SSS.Application.Features.Users.Profile.GetProfile;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Users.Profile.GetProfile;

public sealed class GetProfileEndpoint : EndpointWithoutRequest<GetProfileResponse>
{
    private readonly ISender _sender;
    private readonly AutoMapper.IMapper _mapper;

    public GetProfileEndpoint(ISender sender, AutoMapper.IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/users/profile");
        Description(d => d.WithTags("Users"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var query = new GetProfileQuery { UserId = userId };
        var result = await _sender.Send(query, ct);
        var response = _mapper.Map<GetProfileResponse>(result);

        await SendOkAsync(response, ct);
    }
}
