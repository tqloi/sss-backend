using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyAnswers.GetAllAnswersByResponse;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.GetAllAnswersByResponse
{
    public class GetAllAnswersByResponseEndpoint(ISender sender) 
        : Endpoint<GetAllAnswersByResponseQuery, GetAllAnswersByResponseResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/responses/{responseId}/answers");
            Description(d => d.WithTags("Survey Answers"));
            Summary(s => s.Summary = "Get all answers for a specific response with pagination");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetAllAnswersByResponseQuery req, CancellationToken ct)
        {
            var responseId = Route<long>("responseId");
            var query = req with { ResponseId = responseId };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}