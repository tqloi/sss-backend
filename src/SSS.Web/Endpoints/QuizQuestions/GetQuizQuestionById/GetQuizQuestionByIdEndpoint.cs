using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestions.GetQuizQuestionById;

namespace SSS.Web.Endpoints.QuizQuestions.GetQuizQuestionById
{
    public class GetQuizQuestionByIdEndpoint(ISender sender)
        : Endpoint<GetQuizQuestionByIdQuery, GetQuizQuestionByIdResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-question/{id}");
            Summary(s => s.Summary = "Get a quiz question by id");
            Description(d => d.WithTags("QuizQuestions"));
        }
        public override async Task HandleAsync(GetQuizQuestionByIdQuery req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
