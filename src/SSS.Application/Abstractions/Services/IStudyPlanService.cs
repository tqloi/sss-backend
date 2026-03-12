using SSS.Domain.Entities.Planning;
using SSS.Domain.Enums;

namespace SSS.Application.Abstractions.Services
{
    public interface IStudyPlanService
    {
        /// <summary>
        /// Creates a new StudyPlan for the user + roadmap and seeds one empty module
        /// per RoadmapNode. The plan is initially in GeneratingTasks status.
        /// </summary>
        Task<StudyPlan> CreatePlanWithModulesAsync(string userId, long roadmapId, CancellationToken ct = default);

        /// <summary>
        /// Updates the Status field of a StudyPlan by its ID.
        /// </summary>
        Task SetStatusAsync(long studyPlanId, StudyPlanStatus status, CancellationToken ct = default);
    }
}
