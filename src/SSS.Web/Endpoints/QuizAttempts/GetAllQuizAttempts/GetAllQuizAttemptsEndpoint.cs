using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAttempts.GetAllQuizAttempts;

namespace SSS.Web.Endpoints.QuizAttempts.GetAllQuizAttempts
{
    public class GetAllQuizAttemptsEndpoint(ISender sender)
        : Endpoint<GetAllQuizAttemptsQuery, GetAllQuizAttemptsResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-attempts");
            Summary(s => s.Summary = "Get all quiz attempts with pagination");
            Description(d => d.WithTags("QuizAttempts"));
            Roles("Admin", "User");
        }
        public override async Task HandleAsync(GetAllQuizAttemptsQuery req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
