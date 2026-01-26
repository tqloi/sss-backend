using FastEndpoints;
using MediatR;
using SSS.Application.Features.Quizzes.DeleteQuiz;

namespace SSS.Web.Endpoints.Quizzes.DeleteQuiz
{
    public sealed class DeleteQuizEndpoint(ISender sender)
        : Endpoint<DeleteQuizRequest, DeleteQuizResponse>
    {
        public override void Configure()
        {
            Delete("/api/quiz/{QuizId}");
            Summary(s =>
            {
                s.Summary = "Deletes a quiz by its ID.";
            });
            Roles("Admin");
        }

        public override async Task HandleAsync(DeleteQuizRequest req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            if (!result.IsDeleted)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendAsync(result, cancellation: ct);
        }
    }
}
