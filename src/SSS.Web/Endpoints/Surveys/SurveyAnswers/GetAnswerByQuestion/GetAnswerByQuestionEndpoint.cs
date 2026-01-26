using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyAnswers.GetAnswerByQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.GetAnswerByQuestion
{
    public class GetAnswerByQuestionEndpoint(ISender sender) 
        : EndpointWithoutRequest<GetAnswerByQuestionResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/responses/{responseId}/answers/{questionId}");
            Description(d => d.WithTags("SurveyAnswers"));
            Summary(s => s.Summary = "Get answer for a specific question in a response");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var responseId = Route<long>("responseId");
            var questionId = Route<long>("questionId");
            
            var query = new GetAnswerByQuestionQuery(responseId, questionId);

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}