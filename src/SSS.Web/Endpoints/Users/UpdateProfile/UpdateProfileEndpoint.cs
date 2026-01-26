using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserProfile.UpdateProfile;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Users.UpdateProfile;

public sealed class UpdateProfileEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : Endpoint<UpdateProfileRequest, UpdateProfileResponse>
{
    public override void Configure()
    {
        Put("/api/users/profile");
        Description(d => d.WithTags("Users"));
        Summary(s =>
        {
            s.Summary = "Update current user profile";
            s.Description = "Updates the profile information of the authenticated user. Only provided fields will be updated.";
        });
    }

    public override async Task HandleAsync(UpdateProfileRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        try
        {
            var command = mapper.Map<UpdateProfileCommand>(req);
            command.UserId = userId;

            var result = await sender.Send(command, ct);
            var response = mapper.Map<UpdateProfileResponse>(result);
            await SendOkAsync(response, ct);
        }
        catch (InvalidOperationException ex) when (ex.Message == "User not found")
        {
            await SendNotFoundAsync(ct);
        }
        catch (InvalidOperationException ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(statusCode: 400, cancellation: ct);
        }
    }
}
