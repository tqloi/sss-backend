using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserSubjectStats.CreateUserSubjectStat;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserSubjectStats.Create;

public sealed class CreateUserSubjectStatEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : Endpoint<CreateUserSubjectStatRequest, CreateUserSubjectStatResponse>
{
    public override void Configure()
    {
        Post("/api/user-subject-stats");
        Description(d => d.WithTags("UserSubjectStats"));
        Summary(s =>
        {
            s.Summary = "Create a new user subject stat";
            s.Description = "Creates a new subject statistic for the authenticated user.";
        });
    }

    public override async Task HandleAsync(CreateUserSubjectStatRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        try
        {
            var command = mapper.Map<CreateUserSubjectStatCommand>(req);
            command.UserId = userId;

            var result = await sender.Send(command, ct);
            var response = mapper.Map<CreateUserSubjectStatResponse>(result);
            await SendCreatedAtAsync<GetById.GetUserSubjectStatByIdEndpoint>(
                new { id = result.Id },
                response,
                cancellation: ct
            );
        }
        catch (InvalidOperationException ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(statusCode: 400, cancellation: ct);
        }
    }
}
