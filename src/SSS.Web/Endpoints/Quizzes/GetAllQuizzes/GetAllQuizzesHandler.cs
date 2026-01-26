using FastEndpoints;
using MediatR;
using SSS.Application.Features.Quizzes.GetAllQuizzes;

namespace SSS.Web.Endpoints.Quizzes.GetAllQuizzes
{
    public class GetAllQuizzesHandler(ISender sender)
        : Endpoint<GetAllQuizzesQuery, GetAllQuizzesResult>
    {
        public override void Configure()
        {
            Get("/api/quizzes");
            Summary(s => s.Summary = "Get all quizzes with pagination");
            Description(d => d.WithTags("Quizzes"));
            Roles("Admin", "User");
        }
        public override async Task HandleAsync(GetAllQuizzesQuery req, CancellationToken ct)
        {
            var response =  await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
