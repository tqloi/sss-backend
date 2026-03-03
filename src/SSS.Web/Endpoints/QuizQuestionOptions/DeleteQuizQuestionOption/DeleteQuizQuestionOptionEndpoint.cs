using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestionOptions.DeleteQuizQuestionOption;

namespace SSS.Web.Endpoints.QuizQuestionOptions.DeleteQuizQuestionOption
{
    public class DeleteQuizQuestionOptionEndpoint(ISender sender)
        : Endpoint<DeleteQuizQuestionOptionCommand, DeleteQuizQuestionOptionResult>
    {
        public override void Configure()
        {
            Delete("/api/quiz-question-options/{Id}");
            Summary(s =>
            {
                s.Description = "Deletes a quiz question option.";
            });
            Description(s => s.WithDescription(""));
            AllowAnonymous();
        }
        public override async Task HandleAsync(DeleteQuizQuestionOptionCommand req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result);
        }
    }
}
