using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAttempts.GetQuizAttemptById;

namespace SSS.Web.Endpoints.QuizAttempts.GetQuizAttemptById
{
    public class GetQuizAttemptByIdEndpoint(ISender sender)
        : Endpoint<GetQuizAttemptByIdQuery, GetQuizAttemptByIdResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-attempts/{Id:long}");
            Summary(s => s.Summary = "Get a quiz attempt by ID");
            Description(d => d.WithTags("QuizAttempts"));
        }
        public override async Task HandleAsync(GetQuizAttemptByIdQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result, cancellation: ct);
        }
    }
}
