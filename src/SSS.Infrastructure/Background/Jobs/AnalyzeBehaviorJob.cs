using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Entities.Learning;

namespace SSS.Infrastructure.Background.Jobs
{
    /// <summary>
    /// Background job: analyze a Learning Behavior survey via AI and persist the result.
    /// Triggered by: ISurveyJobDispatcher.DispatchBehaviorAnalysis
    /// </summary>
    public class AnalyzeBehaviorJob(
        ISurveyAnalysisService surveyAnalysis,
        IAppDbContext db,
        IMapper mapper,
        ILogger<AnalyzeBehaviorJob> logger)
    {
        public async Task ExecuteAsync(long responseId, CancellationToken ct = default)
        {
            logger.LogInformation("[AnalyzeBehaviorJob] Starting for responseId={ResponseId}", responseId);

            var behavior = await surveyAnalysis.AnalyzeBehaviorAsync(responseId, ct);

            var existingBehavior = await db.UserLearningBehaviors.FirstOrDefaultAsync
        (x => x.UserId == behavior.UserId, ct);

            if (existingBehavior == null)
            {
                db.UserLearningBehaviors.Add(behavior);
            }
            else
            {
                mapper.Map(behavior, existingBehavior);
            }

            await db.SaveChangesAsync(ct);

            logger.LogInformation("[AnalyzeBehaviorJob] Completed. UserLearningBehavior saved for userId={UserId}", behavior.UserId);
        }
    }

    public class BehaviorProfile : Profile
    {
        public BehaviorProfile()
        {
            CreateMap<UserLearningBehavior, UserLearningBehavior>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())       
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); 
        }
    }
}
