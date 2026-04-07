using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserSubjectStats.DeleteUserSubjectStat;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserSubjectStats.Delete;

public sealed class DeleteUserSubjectStatEndpoint(
    ISender sender
) : Endpoint<DeleteUserSubjectStatRequest>
{
    public override void Configure()
    {
        Delete("/api/user-subject-stats/{id}");
        Description(d => d.WithTags("UserSubjectStats"));
        Summary(s =>
        {
            s.Summary = "Delete a user subject stat";
            s.Description = "Deletes a subject statistic for the authenticated user.";
        });
    }

    public override async Task HandleAsync(DeleteUserSubjectStatRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var command = new DeleteUserSubjectStatCommand { Id = req.Id, UserId = userId };
        var deleted = await sender.Send(command, ct);

        if (!deleted)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}
