using FastEndpoints;
using MediatR;
using SSS.Application.Features.Subscriptions.GetUserMembership;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Subscriptions.GetUserMembership;

public sealed class GetUserMembershipEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : EndpointWithoutRequest<GetUserMembershipResponse>
{
    public override void Configure()
    {
        Get("/api/users/membership");
        Description(d => d.WithTags("Users"));
        Summary(s =>
        {
            s.Summary = "Get current user membership";
            s.Description = "Returns membership/subscription information of the authenticated user.";
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
            var result = await sender.Send(new GetUserMembershipQuery { UserId = userId }, ct);
            var response = mapper.Map<GetUserMembershipResponse>(result);
            await SendOkAsync(response, ct);
        }
        catch (InvalidOperationException)
        {
            await SendNotFoundAsync(ct);
        }
    }
}
