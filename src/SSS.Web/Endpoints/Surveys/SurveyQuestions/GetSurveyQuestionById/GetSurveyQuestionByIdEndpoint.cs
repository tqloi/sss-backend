using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.GetSurveyQuestionById
{
    public class GetSurveyQuestionByIdEndpoint(ISender sender): EndpointWithoutRequest<GetSurveyQuestionByIdResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/{surveyId}/questions/{questionId}");
            Description(d => d.WithTags("SurveyQuestions"));
            Summary(s => s.Summary = "Get question detail by ID");
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var surveyId = Route<int>("surveyId");
            var questionId = Route<int>("questionId");
            var response = await sender.Send(new GetSurveyQuestionByIdQuery(questionId,surveyId), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
