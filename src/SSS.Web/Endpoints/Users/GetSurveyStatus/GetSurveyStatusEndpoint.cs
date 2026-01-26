using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserProfile.GetSurveyStatus;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Users.GetSurveyStatus;

public sealed class GetSurveyStatusEndpoint(
    ISender sender
) : EndpointWithoutRequest<GetSurveyStatusResponse>
{
    public override void Configure()
    {
        Get("/api/users/survey-status");
        Description(d => d.WithTags("Users"));
        Summary(s =>
        {
            s.Summary = "Get survey completion status for current user";
            s.Description = "Checks if the authenticated user has completed the initial survey (LEARNING_BEHAVIOR). Returns redirect information if survey needs to be completed.";
            s.Response<GetSurveyStatusResponse>(200, "Survey status retrieved successfully");
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

        var query = new GetSurveyStatusQuery { UserId = userId };
        var result = await sender.Send(query, ct);
        await SendOkAsync(result, ct);
    }
}
