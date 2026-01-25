using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionById;
using SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionById
{
    public class GetSurveyQuestionOptionByIdEndpoint(ISender sender): EndpointWithoutRequest<GetSurveyQuestionOptionByIdResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/question/options/{optionId}");
            Description(d => d.WithTags("Survey Options"));
            Summary(s => s.Summary = "Get option detail by ID");
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var optionId = Route<int>("optionId");
            var response = await sender.Send(new GetSurveyQuestionOptionByIdQuery(optionId), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
