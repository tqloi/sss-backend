using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.GetPendingTriggerSurvey;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.GetPendingTriggerSurvey
{
    /// <summary>
    /// HTTP request model — binds the ?triggerType= query parameter.
    /// </summary>
    public class GetPendingTriggerSurveyRequest
    {
        [QueryParam]
        public string TriggerType { get; set; } = default!;
    }

    public class GetPendingTriggerSurveyEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<GetPendingTriggerSurveyRequest, GetPendingTriggerSurveyResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/surveytriggermapping/pending-trigger");
            Description(d => d.WithTags("SurveyTriggerMappings"));
            Summary(s =>
            {
                s.Summary = "Check if there is a pending survey for a given trigger type for the current user";
                s.Description =
                    "Returns the first eligible survey based on TriggerType, " +
                    "respecting MaxAttempts and CooldownDays configured by Analyst. " +
                    "TriggerType values: ON_REGISTER | ON_START_ROADMAP | ON_COMPLETE_MODULE";
            });
        }

        public override async Task HandleAsync(GetPendingTriggerSurveyRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await sender.Send(
                new GetPendingTriggerSurveyQuery(req.TriggerType, userId!), ct);

            await SendOkAsync(result, ct);
        }
    }
}
