using AutoMapper;
using SSS.Application.Features.StudyPlans.StudyPlans.Common;
using SSS.Domain.Entities.Planning;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.Common
{
    public class StudyPlanMappingProfile : Profile
    {
        public StudyPlanMappingProfile()
        {
            // Entity to DTO
            CreateMap<StudyPlan, StudyPlanDto>()
                .ForMember(dest => dest.RoadmapName, opt => opt.MapFrom(src => src.Roadmap.Title))
                .ForMember(dest => dest.Modules, opt => opt.MapFrom(src => src.Modules));

            CreateMap<StudyPlanModule, StudyModuleDto>()
                .ForMember(dest => dest.RoadmapNodeName, opt => opt.MapFrom(src => src.RoadmapNode.Title));
        }
    }
}
