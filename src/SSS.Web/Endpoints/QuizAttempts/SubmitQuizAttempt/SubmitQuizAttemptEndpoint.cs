using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAttempts.SubmitQuizAttemp;
using System.Security.Claims;

namespace SSS.Web.Endpoints.QuizAttempts.SubmitQuizAttempt
{
    public class SubmitQuizAttemptEndpoint(ISender sender)
        : Endpoint<SubmitQuizAttemptCommand, SubmitQuizAttemptResult>
    {
        public override void Configure()
        {
            Post("/api/quiz-attempts/{Id:long}/submit");
            Summary(s => s.Summary = "Submit a quiz attempt with answers");
            Description(d => d.WithTags("QuizAttempts"));
            Roles("User");
        }

        public override async Task HandleAsync(SubmitQuizAttemptCommand req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result, cancellation: ct);
        }
    }
}
