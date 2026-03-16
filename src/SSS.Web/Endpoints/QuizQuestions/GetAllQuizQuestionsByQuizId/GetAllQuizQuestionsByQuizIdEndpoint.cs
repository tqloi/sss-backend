using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestions.GetAllQuizQuestionsByQuizId;

namespace SSS.Web.Endpoints.QuizQuestions.GetAllQuizQuestionsByQuizId
{
    public class GetAllQuizQuestionsByQuizIdEndpoint(ISender sender)
        : Endpoint<GetAllQuizQuestionsByQuizIdQuery, GetAllQuizQuestionsByQuizIdResult>
    {
        public override void Configure()
        {
            Get("/api/quiz/{quizId}/questions");
            Summary(s => s.Summary = "Get all quiz questions and options by quiz id");
            Description(d => d.WithTags("QuizQuestions"));
        }

        public override async Task HandleAsync(GetAllQuizQuestionsByQuizIdQuery req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
