using MediatR;
using SSS.Application.Features.Content.LearningCategory.Create;
using FastEndpoints;

namespace SSS.Web.Endpoints.Content.LearningCategory.Create
{
    public class CreateLearningCategoryEndpoint(ISender sender)
        : Endpoint<CreateLearningCategoryCommand, CreateLearningCategoryResult>
    {
        public override void Configure()
        {
            Post("/api/learning-categories");
            Summary(s => s.Summary = "Create a new learning category");
            Roles("Admin");
        }
        public override async Task HandleAsync(
            CreateLearningCategoryCommand req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendOkAsync(result, ct);
        }
    }
}
