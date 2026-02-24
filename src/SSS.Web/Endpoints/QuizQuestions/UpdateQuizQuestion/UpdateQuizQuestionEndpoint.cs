using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAnswers.UpdateQuizAnswer;
using SSS.Application.Features.QuizQuestions.UpdateQuizQuestion;

namespace SSS.Web.Endpoints.QuizQuestions.UpdateQuizQuestion
{
    public class UpdateQuizQuestionEndpoint(ISender sender)
        : Endpoint<UpdateQuizQuestionCommand, UpdateQuizQuestionResult>
    {
        public override void Configure()
        {
            Put("/api/quiz-question");
            Summary(s => s.Summary = "Update a quiz question");
            Description(d => d.WithTags("QuizQuestions"));
        }
        public override async Task HandleAsync(UpdateQuizQuestionCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
