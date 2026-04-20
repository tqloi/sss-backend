using AutoMapper;
using SSS.Application.Features.Reviews.Common;
using SSS.Domain.Entities.Content;

namespace SSS.Application.Features.Reviews.Common.MappingProfile
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.RoadmapTitle, opt => opt.MapFrom(src => src.Roadmap.Title))
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src =>
                    src.Reviewer != null ? src.Reviewer.UserName : null));
        }
    }
}
