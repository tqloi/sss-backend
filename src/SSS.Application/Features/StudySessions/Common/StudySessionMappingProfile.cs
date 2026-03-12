using AutoMapper;
using SSS.Domain.Entities.Tracking;

namespace SSS.Application.Features.StudySessions.Common
{
    public class StudySessionMappingProfile : Profile
    {
        public StudySessionMappingProfile()
        {
            // Detail mapping
            CreateMap<StudySession, SessionDetailDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.EndedReason, o => o.MapFrom(s => s.EndedReason != null ? s.EndedReason.ToString() : null))
                .ForMember(d => d.Node, o => o.MapFrom(s => s.Node))
                .ForMember(d => d.Plan, o => o.MapFrom(s => s.StudyPlan));

            // Sub-DTOs
            CreateMap<SSS.Domain.Entities.Content.RoadmapNode, SessionNodeDto>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title));

            CreateMap<SSS.Domain.Entities.Planning.StudyPlan, SessionPlanDto>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Roadmap != null ? s.Roadmap.Title : ""));

            CreateMap<SSS.Domain.Entities.Planning.TaskItem, SessionTaskDto>()
                .ForMember(d => d.IsCompleted, o => o.MapFrom(s => s.Status == SSS.Domain.Enums.TaskStatus.Completed));
        }
    }
}
