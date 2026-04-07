using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAttempts.CreateQuizAttempt;
using System.Security.Claims;

namespace SSS.Web.Endpoints.QuizAttempts.CreateQuizAttempt
{
    public class CreateQuizAttemptEndpoint(ISender sender)
        : Endpoint<CreateQuizAttemptCommand, CreateQuizAttemptResult>
    {
        public override void Configure()
        {
            Post("/api/quiz-attempts");
            Summary(s => s.Summary = "Create a new quiz attempt");
            Description(d => d.WithTags("QuizAttempts"));
            Roles("User");
        }

        public override async Task HandleAsync(CreateQuizAttemptCommand req, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            req.CreateQuizAttempt.UserId = userId!;

            var result = await sender.Send(req, ct);
            await SendAsync(result, cancellation: ct);
        }
    }
}
