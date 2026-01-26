using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using SSS.Application.Features.Surveys.SurveyResponses.CreateSurveyResponse;
using SSS.Application.Features.Surveys.SurveyResponses.SubmitResponse;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.SubmitResponse
{
    public class SubmitResponseEndpoint(ISender sender, IHttpContextAccessor httpContext) : Endpoint<SubmitResponseCommand, SubmitResponseResponse>
    {
        public override void Configure()
        {
            Patch("/api/surveys/{surveyId}/responses/{responseId}");
            Description(d => d.WithTags("SurveyResponses"));
            
            Summary(s => s.Summary = "Submit a survey response");
        }


        public override async Task HandleAsync(SubmitResponseCommand req, CancellationToken ct)
        {
            var surveyId = Route<long>("SurveyId");
            var responseId = Route<long>("responseId");
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var command = req with
            {
                SurveyId = surveyId,
                ResponseId = responseId,
                UserId = userId!
            };


            var res = await sender.Send(command, ct);
            await SendOkAsync(res, ct);
        }
    }
}
