using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey;
using SSS.Application.Features.Surveys.SurveyResponses.GetResponsesOfSurvey;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.GetResponsesOfSurvey
{
    public class GetResponsesOfSurveyEndpoint(ISender sender) : EndpointWithoutRequest<GetResponseOfSurveyResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/{surveyId}/responses");
            Summary(s => s.Summary = "Get List responses by Survey ID");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var surveyId = Route<long>("surveyId");
            var response = await sender.Send(new GetResponseOfSurveyQuery(surveyId, 1, 10), ct);
            await SendAsync(response, cancellation: ct);
        }

    }
}
