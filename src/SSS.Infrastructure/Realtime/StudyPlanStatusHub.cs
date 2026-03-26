using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SSS.Infrastructure.Realtime
{
    [Authorize]
    public class StudyPlanStatusHub : Hub
    {
        public const string HubPath = "/hubs/study-plan-status";

        public Task JoinStudyPlanGroup(long studyPlanId)
            => Groups.AddToGroupAsync(Context.ConnectionId, BuildGroupName(studyPlanId));

        public Task LeaveStudyPlanGroup(long studyPlanId)
            => Groups.RemoveFromGroupAsync(Context.ConnectionId, BuildGroupName(studyPlanId));

        public static string BuildGroupName(long studyPlanId) => $"study-plan:{studyPlanId}";
    }
}