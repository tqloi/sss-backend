namespace SSS.Application.Features.Content.LearningCategory.Common
{
    using AutoMapper;
    using SSS.Domain.Entities.Content;
    public class LearningCategoryMappingProfile : Profile
    {
        public LearningCategoryMappingProfile()
        {
            CreateMap<LearningCategory, LearningCategoryDTO>();
        }
    }
}
