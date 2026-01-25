using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.LearningCategory.Update;

namespace SSS.Web.Endpoints.Content.LearningCategory.Update
{
    public class UpdateLearningCategoryEndpoint(ISender sender)
        : Endpoint<UpdateLearningCategoryCommand, UpdateLearningCategoryResult>
    {
        public override void Configure()
        {
            Put("/api/learning-categories/{id}");
            Summary(s => s.Summary = "Update an existing learning category");
            Roles("Admin");
        }

        public override async Task HandleAsync(
            UpdateLearningCategoryCommand req,
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
