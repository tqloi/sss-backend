using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAnswers.DeleteQuizAnswers;

namespace SSS.Web.Endpoints.QuizAnswers.DeleteQuizAnswers
{
    public class DeleteQuizAnswersEndpoint(ISender sender)
        : Endpoint<DeleteQuizAnswersCommand, DeleteQuizAnswersResult>
    {
        public override void Configure()
        {
            Delete("/api/quiz-answer/{id}");
            Summary(s => s.Summary = "Remove a quiz answer by ID");
            Description(d => d.WithTags("QuizAnswers"));
            AllowAnonymous();
        }
        public override async Task HandleAsync(DeleteQuizAnswersCommand req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result);
        }
    }
}
