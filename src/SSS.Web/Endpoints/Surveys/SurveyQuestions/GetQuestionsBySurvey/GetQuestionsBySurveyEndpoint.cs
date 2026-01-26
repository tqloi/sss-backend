using FastEndpoints;
using MediatR;
using Org.BouncyCastle.Ocsp;
using SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey;
using SSS.Application.Features.Surveys.Surveys.GetSurveyById;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.GetQuestionsBySurvey
{
    public class GetQuestionsBySurveyEndpoint(ISender sender): EndpointWithoutRequest<GetQuestionsBySurveyResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/{surveyId}/questions");
            Description(d => d.WithTags("SurveyQuestions"));
            Summary(s => s.Summary = "Get List questions by Survey ID");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var surveyId = Route<int>("surveyId");
            var response = await sender.Send(new GetQuestionsBySurveyQuery(surveyId,1,10), ct);
            await SendAsync(response, cancellation: ct);
        }
        

    }
}
