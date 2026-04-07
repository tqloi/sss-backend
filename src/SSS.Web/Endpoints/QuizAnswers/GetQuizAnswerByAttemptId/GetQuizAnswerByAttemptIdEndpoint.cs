using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAnswers.GetQuizAnswerByAttemptId;

namespace SSS.Web.Endpoints.QuizAnswers.GetQuizAnswerByAttemptId
{
    public class GetQuizAnswerByAttemptIdEndpoint(ISender sender)
        : Endpoint<GetQuizAnswerByAttemptIdQuery, GetQuizAnswerByAttemptIdResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-answer/attempt/{attemptId}/question/{questionId}");
            Summary(s => s.Summary = "Get a quiz answer by attempt ID and question ID");

        }
        public override async Task HandleAsync(GetQuizAnswerByAttemptIdQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result);
        }
    }
}
