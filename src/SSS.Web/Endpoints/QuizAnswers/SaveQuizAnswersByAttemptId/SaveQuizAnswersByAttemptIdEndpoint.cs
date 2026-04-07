using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAnswers.SaveQuizAnswersByAttemptId;

namespace SSS.Web.Endpoints.QuizAnswers.SaveQuizAnswersByAttemptId
{
    public class SaveQuizAnswersByAttemptIdEndpoint(ISender sender)
        : Endpoint<SaveQuizAnswersByAttemptIdCommand, SaveQuizAnswersByAttemptIdResult>
    {
        public override void Configure()
        {
            Post("/api/quiz-answers/attempt/{AttemptId:long}");
            Summary(s => s.Summary = "Save/update quiz answers by attempt ID");
            Description(d => d.WithTags("QuizAnswers"));
            Roles("User");
        }

        public override async Task HandleAsync(SaveQuizAnswersByAttemptIdCommand req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result, cancellation: ct);
        }
    }
}
