using MediatR;

namespace SSS.Application.Features.Content.LearningCategory.GetAll
{
    public sealed record GetAllLearningCategoryQuery(
        int PageIndex,
        int PageSize
    ) : IRequest<GetAllLearningCategoryResult>;
}
