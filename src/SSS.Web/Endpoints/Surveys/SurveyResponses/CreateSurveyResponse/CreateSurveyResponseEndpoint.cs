using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyResponses.CreateSurveyResponse;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.CreateSurveyResponse
{
    public class CreateSurveyResponseEndpoint(ISender sender, IHttpContextAccessor httpContext): Endpoint<CreateSurveyResponseCommand, CreateSurveyResponseResponse>
    {
        public override void Configure()
        {
            Post("/api/surveys/{surveyId}/responses");
            Description(d => d.WithTags("Survey Responses"));
            Summary(s => s.Summary = "Create a new survey response");
        }
        

        public override async Task HandleAsync(CreateSurveyResponseCommand req, CancellationToken ct)
        {
            var surveyId = Route<long>("surveyId");
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var command = req with
            {
                SurveyId = surveyId,
                UserId = userId!
            };
           

            var res = await sender.Send(command, ct);
            await SendOkAsync(res, ct);
        }
    }
    
    }

