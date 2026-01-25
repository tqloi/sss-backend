using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById;
using SSS.Application.Features.Surveys.Surveys.GetSurveyById;

namespace SSS.Web.Endpoints.Surveys.Surveys.GetSurveyById
{
    public class GetSurveyByIdEndpoint(ISender sender) : EndpointWithoutRequest<GetSurveyByIdResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/{surveyId}");
            Description(d => d.WithTags("Surveys"));
            Summary(s => s.Summary = "Get survey detail by ID");
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var surveyId = Route<int>("surveyId");
            var response = await sender.Send(new GetSurveyByIdQuery(surveyId), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
