using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserSubjectStats.GetUserSubjectStatById;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserSubjectStats.GetById;

public sealed class GetUserSubjectStatByIdEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : Endpoint<GetUserSubjectStatByIdRequest, GetUserSubjectStatByIdResponse>
{
    public override void Configure()
    {
        Get("/api/user-subject-stats/{id}");
        Description(d => d.WithTags("UserSubjectStats"));
        Summary(s =>
        {
            s.Summary = "Get user subject stat by ID";
            s.Description = "Returns a specific subject statistic by ID for the authenticated user.";
        });
    }

    public override async Task HandleAsync(GetUserSubjectStatByIdRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var query = new GetUserSubjectStatByIdQuery { Id = req.Id, UserId = userId };
        var result = await sender.Send(query, ct);

        if (result is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = mapper.Map<GetUserSubjectStatByIdResponse>(result);
        await SendOkAsync(response, ct);
    }
}
