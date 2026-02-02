using SSS.Application.Features.StudyPlans.TaskItems.Common;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.CreateTaskList
{
    public class CreateTaskListRequest
    {
        public List<TastItemInput> Tasks { get; set; } = new();
    }
}
