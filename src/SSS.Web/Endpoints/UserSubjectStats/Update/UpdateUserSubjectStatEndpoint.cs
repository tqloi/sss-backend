using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserSubjectStats.UpdateUserSubjectStat;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserSubjectStats.Update;

public sealed class UpdateUserSubjectStatEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : Endpoint<UpdateUserSubjectStatRequest, UpdateUserSubjectStatResponse>
{
    public override void Configure()
    {
        Put("/api/user-subject-stats/{id}");
        Description(d => d.WithTags("UserSubjectStats"));
        Summary(s =>
        {
            s.Summary = "Update a user subject stat";
            s.Description = "Updates an existing subject statistic for the authenticated user.";
        });
    }

    public override async Task HandleAsync(UpdateUserSubjectStatRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        try
        {
            var command = mapper.Map<UpdateUserSubjectStatCommand>(req);
            command.Id = req.Id;
            command.UserId = userId;

            var result = await sender.Send(command, ct);
            var response = mapper.Map<UpdateUserSubjectStatResponse>(result);
            await SendOkAsync(response, ct);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("not found"))
            {
                await SendNotFoundAsync(ct);
            }
            else
            {
                AddError(ex.Message);
                await SendErrorsAsync(statusCode: 400, cancellation: ct);
            }
        }
    }
}
