using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestions.DeleteQuizQuestion;

namespace SSS.Web.Endpoints.QuizQuestions.DeleteQuizQuestion
{
    public class DeleteQuizQuestionEndpoint(ISender sender)
        : Endpoint<DeleteQuizQuestionCommand, DeleteQuizQuestionResult>
    {
        public override void Configure()
        {
            Delete("/api/quiz-question/{id}");
            Summary(s => s.Summary = "Delete a quiz question by id");
            Description(d => d.WithTags("QuizQuestions"));
        }
        public override async Task HandleAsync(DeleteQuizQuestionCommand req, CancellationToken ct)
        => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
