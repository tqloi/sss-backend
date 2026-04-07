using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.GetSurveyFieldSemanticByQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyFieldSemantics.GetSurveyFieldSemanticByQuestion
{
    public class GetSurveyFieldSemanticByQuestionEndpoint(ISender sender): Endpoint<GetSurveyFieldSemanticByQuestionQuery, GetSurveyFieldSemanticByQuestionResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/question/surveyfieldsemantic");
            Description(d => d.WithTags("SurveyFieldSemantics"));
            Summary(s => s.Summary = "Get survey field semantic by Question Id");
            Roles("Analyst");
        }
        public override async Task HandleAsync(GetSurveyFieldSemanticByQuestionQuery req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    
    }
}
