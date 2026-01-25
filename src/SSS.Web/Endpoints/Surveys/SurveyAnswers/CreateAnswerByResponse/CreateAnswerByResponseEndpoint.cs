using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyAnswers.CreateAnswerByResponse;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.CreateAnswerByResponse
{
    public class CreateAnswerByResponseEndpoint(ISender sender) 
        : Endpoint<CreateAnswerByResponseCommand, CreateAnswerByResponseResponse>
    {
        public override void Configure()
        {
            Post("/api/surveys/responses/{responseId}/answers");
            Description(d => d.WithTags("Survey Answers"));
            Summary(s => s.Summary = "Create or update answer for a survey response");
        }

        public override async Task HandleAsync(
            CreateAnswerByResponseCommand req, 
            CancellationToken ct)
        {
            var responseId = Route<long>("responseId");
            var command = req with { ResponseId = responseId };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}