using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.LearningCategory.Delete;

namespace SSS.Web.Endpoints.Content.LearningCategory.Delete
{
    public class DeleteLearningCategoryEndpoint(ISender sender)
        : Endpoint<DeleteLearningCategoryCommand, DeleteLearningCategoryResult>
    {
        public override void Configure()
        {
            Delete("/api/learning-categories/{id}");
            Summary(s => s.Summary = "Delete a learning category");
            Description(d => d.WithTags("LearningCategories"));
            Roles("Admin");
        }

        public override async Task HandleAsync(
            DeleteLearningCategoryCommand req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);

            if (!result.Success)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendOkAsync(result, ct);
        }
    }
}
