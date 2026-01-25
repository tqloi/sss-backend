using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionById;
using SSS.Application.Features.Surveys.SurveyQuestions.CreateSurveyQuestion;
using SSS.Application.Features.Surveys.SurveyResponses.GetResponseById;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.GetResponseById
{
    public class GetResponseByIdEndpoint(ISender sender) : EndpointWithoutRequest<GetResponseByIdResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/response/{responseId}");
            Description(d => d.WithTags("Survey Responses"));
            Summary(s => s.Summary = "Get response detail by ID");
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var responseId = Route<long>("responseId");
            var response = await sender.Send(new GetResponseByIdQuery(responseId), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
