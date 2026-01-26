using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.LearningCategory.GetAll;

namespace SSS.Web.Endpoints.Content.LearningCategory.GetAll
{
    public class GetAllLearningCategoryEndpoint(ISender sender)
        : Endpoint<GetAllLearningCategoryQuery, GetAllLearningCategoryResult>
    {
        public override void Configure()
        {
            Get("/api/learning-categories");
            Summary(s => s.Summary = "Get all learning categories with pagination");
            Description(d => d.WithTags("LearningCategories"));
            AllowAnonymous();
        }

        public override async Task HandleAsync(
            GetAllLearningCategoryQuery req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendOkAsync(result, ct);
        }
    }
}
