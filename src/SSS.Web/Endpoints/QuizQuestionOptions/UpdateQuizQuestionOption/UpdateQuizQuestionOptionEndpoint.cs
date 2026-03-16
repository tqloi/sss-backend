using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestionOptions.UpdateQuizQuestionOption;

namespace SSS.Web.Endpoints.QuizQuestionOptions.UpdateQuizQuestionOption
{
    public class UpdateQuizQuestionOptionEndpoint(ISender sender)
        : Endpoint<UpdateQuizQuestionOptionCommand, UpdateQuizQuestionOptionResult>
    {
        public override void Configure()
        {
            Put("/api/quiz-question-options/{Id}");
            Summary(s =>
            {
                s.Summary = "Update a quiz question option.";
                s.Description = "Updates the details of an existing quiz question option identified by its ID.";
            });
            Roles("ContentManager");
        }

        public override async Task HandleAsync(UpdateQuizQuestionOptionCommand req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result, cancellation: ct);
        }
    }
}
