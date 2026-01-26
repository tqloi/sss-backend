using FastEndpoints;
using MediatR;
using SSS.Application.Features.Quizzes.GetAllQuizNode;

namespace SSS.Web.Endpoints.Quizzes.GetAllQuizzes
{
    public class GetAllQuizzesHandler(ISender sender)
        : Endpoint<GetAllQuizzesRequest, GetAllQuizzesResponse>
    {
        public override void Configure()
        {
            Get("/api/quizzes");
            Summary(s => s.Summary = "Get all quizzes with pagination");
            Roles("Admin", "User");
        }
        public override async Task HandleAsync(GetAllQuizzesRequest req, CancellationToken ct)
        {
            var response =  await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
