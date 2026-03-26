using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAttempts.GetCurrentQuizAttemptByUser;
using System.Security.Claims;

namespace SSS.Web.Endpoints.QuizAttempts.GetCurrentQuizAttemptByUser
{
    public class GetCurrentQuizAttemptByUserEndpoint(ISender sender)
        : Endpoint<GetCurrentQuizAttemptByUserQuery, GetCurrentQuizAttemptByUserResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-attempts/currentByModule/{ModuleId:long}");
            Summary(s => s.Summary = "Get current in-progress quiz attempt by module");
            Description(d => d.WithTags("QuizAttempts"));
            Roles("User");
        }

        public override async Task HandleAsync(GetCurrentQuizAttemptByUserQuery req, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            req.UserId = userId!;

            var result = await sender.Send(req, ct);
            await SendAsync(result, cancellation: ct);
        }
    }
}
