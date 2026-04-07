using FastEndpoints;
using MediatR;
using SSS.Application.Features.Quizzes.GetQuizById;

namespace SSS.Web.Endpoints.Quizzes.GetQuizById
{
    public class GetQuizByIdEndpoint(ISender sender)
        : Endpoint<GetQuizByIdQuery, GetQuizByIdResult>
    {
        public override void Configure()
        {
            Get("/api/quiz/{QuizId}");
            Summary(s =>
            {
                s.Summary = "Retrieves a quiz by its ID.";
            });
            Description(d => d.WithTags("Quizzes"));

            // Roles("Admin", "Instructor", "Student");
        }

        public override async Task HandleAsync(GetQuizByIdQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            if (result.QuizDto == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }
            await SendAsync(result, cancellation: ct);
        }
    }
}
