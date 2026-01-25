using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionByQuestion;
using SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionByQuestion
{
    public class GetSurveyQuestionOptionByQuestionEndpoint(ISender sender): Endpoint<GetSurveyQuestionOptionByQuestionQuery, GetSurveyQuestionOptionByQuestionResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/question/option");
            Description(d => d.WithTags("Survey Options"));
            Summary(s => s.Summary = "Get List options by Question Id");
        }
        public override async Task HandleAsync(GetSurveyQuestionOptionByQuestionQuery req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
