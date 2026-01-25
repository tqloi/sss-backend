using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.Surveys.TakeSurvey;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Surveys.Surveys.TakeSurvey
{
    public class TakeSurveyEndpoint(ISender sender, IHttpContextAccessor httpContext) 
        : Endpoint<TakeSurveyCommand, TakeSurveyResponse>
    {
        public override void Configure()
        {
            Post("/api/surveys/{surveyId}/take");
            Description(d => d.WithTags("Surveys"));
            Summary(s => s.Summary = "Take a survey - submit answers as draft or final submission");
        }

        public override async Task HandleAsync(TakeSurveyCommand req, CancellationToken ct)
        {
            var surveyId = Route<long>("surveyId");
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = req with
            {
                SurveyId = surveyId,
                UserId = userId!
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}