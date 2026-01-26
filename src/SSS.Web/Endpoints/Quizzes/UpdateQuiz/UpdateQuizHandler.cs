using FastEndpoints;
using MediatR;
using SSS.Application.Features.Quizzes.UpdateQuizNode;

namespace SSS.Web.Endpoints.Quizzes.UpdateQuiz
{
    public class UpdateQuizHandler(ISender sender)
        : Endpoint<UpdateQuizCommand, UpdateQuizResult>
    {
        public override void Configure()
        {
            Put("/api/quiz/{QuizId}");
            Summary(s => s.Summary = "Update an existing quiz by its ID.");
            Description(d => d.WithTags("Quizzes"));
            Roles("Admin");
        }
        public override async Task HandleAsync(UpdateQuizCommand req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            
            await SendAsync(result, cancellation: ct);
        }
    }
}
