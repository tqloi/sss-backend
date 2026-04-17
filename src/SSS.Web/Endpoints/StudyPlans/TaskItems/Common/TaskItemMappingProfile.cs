using AutoMapper;
using SSS.Application.Features.StudyPlans.TaskItems.Common;
using SSS.Application.Features.StudyPlans.TaskItems.CreateTask;
using SSS.Application.Features.StudyPlans.TaskItems.UpdateTask;
using SSS.Domain.Entities.Planning;
using SSS.Web.Endpoints.StudyPlans.TaskItems.CreateTask;
using SSS.Web.Endpoints.StudyPlans.TaskItems.UpdateTask;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.Common
{
    public class TaskItemMappingProfile : Profile
    {
        public TaskItemMappingProfile()
        {
            CreateMap<CreateTaskRequest, CreateTaskCommand>();
            CreateMap<UpdateTaskRequest, UpdateTaskCommand>();

            // Entity to DTO
            CreateMap<TaskItem, TaskItemDtos>()
                .ForMember(dest => dest.ExpectedOutput, opt => opt.MapFrom(src => src.ExpectOutput));

            // DTO to Entity
            CreateMap<TaskItemDtos, TaskItem>()
                .ForMember(dest => dest.ExpectOutput, opt => opt.MapFrom(src => src.ExpectedOutput))
                .ForMember(dest => dest.StudyPlanModule, opt => opt.Ignore())
                .ForMember(dest => dest.SessionTasks, opt => opt.Ignore());
        }
    }
}
