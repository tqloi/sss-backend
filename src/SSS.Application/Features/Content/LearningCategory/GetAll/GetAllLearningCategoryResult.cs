using SSS.Application.Common.Dtos;
using SSS.Application.Features.Content.LearningCategory.Common;

namespace SSS.Application.Features.Content.LearningCategory.GetAll
{
    public sealed record GetAllLearningCategoryResult(
        PaginatedResponse<LearningCategoryDTO> Categories
    );
}
