using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAttempts.GetQuizzesByQuizAttempt;

namespace SSS.Web.Endpoints.QuizAttempts.GetQuizzesByQuizAttempt
{
    public class GetQuizzesByQuizAttemptEndpoint(ISender sender)
        : Endpoint<GetQuizzesByQuizAttemptQuery, GetQuizzesByQuizAttemptResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-attempts/{AttemptId:long}/questions");
            Summary(s => s.Summary = "Get all questions with options and user answers for an attempt");
            Description(d => d.WithTags("QuizAttempts"));
            Roles("User");
        }

        public override async Task HandleAsync(GetQuizzesByQuizAttemptQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result, cancellation: ct);
        }
    }
}
