using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserProfile.GetProfile;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Users.GetProfile;

public sealed class GetProfileEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : EndpointWithoutRequest<GetProfileResponse>
{
    public override void Configure()
    {
        Get("/api/users/profile");
        Description(d => d.WithTags("Users"));
        Summary(s =>
        {
            s.Summary = "Get current user profile";
            s.Description = "Returns the profile information of the authenticated user.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        try
        {
            var query = new GetProfileQuery { UserId = userId };
            var result = await sender.Send(query, ct);
            var response = mapper.Map<GetProfileResponse>(result);
            await SendOkAsync(response, ct);
        }
        catch (InvalidOperationException ex)
        {
            await SendNotFoundAsync(ct);
        }
    }
}
