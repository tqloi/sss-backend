using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserProfile.ChangePassword;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Users.ChangePassword;

public sealed class ChangePasswordEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : Endpoint<ChangePasswordRequest, ChangePasswordResponse>
{
    public override void Configure()
    {
        Put("/api/users/password");
        Description(d => d.WithTags("Users"));
        Summary(s =>
        {
            s.Summary = "Change user password";
            s.Description = "Changes the password for the authenticated user. Requires current password verification.";
        });
    }

    public override async Task HandleAsync(ChangePasswordRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        try
        {
            var command = mapper.Map<ChangePasswordCommand>(req);
            command.UserId = userId;

            var result = await sender.Send(command, ct);
            var response = mapper.Map<ChangePasswordResponse>(result);
            await SendOkAsync(response, ct);
        }
        catch (InvalidOperationException ex) when (ex.Message == "User not found")
        {
            await SendNotFoundAsync(ct);
        }
        catch (InvalidOperationException ex) when (ex.Message == "Current password is incorrect")
        {
            AddError("CurrentPassword", ex.Message);
            await SendErrorsAsync(statusCode: 400, cancellation: ct);
        }
        catch (InvalidOperationException ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(statusCode: 400, cancellation: ct);
        }
    }
}
