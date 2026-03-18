using SSS.Application.Features.AI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Abstractions.External.AI.PipeLine
{
    public interface IPipeLine
    {
        Task IngestAsync(string studyplanId, string userId, IEnumerable<(string Text, string? Source)> chunks, CancellationToken ct = default);
        Task IngestBehaviorAsync(string studyplanId, string userId, IEnumerable<(string Text, string? Source)> chunks, CancellationToken ct = default);
        Task<string> AskAsync(string question, CancellationToken ct = default);

        Task<string> BuildStudyPlanContextAsync(string userId, string studyplanId, CancellationToken ct = default);
        Task<string> GenerateStudyPlanAsync(string userId,string studyplanId, object roadmap, object roadmapnode, CancellationToken ct = default);
        Task<string> GenerateRoadmapAsync(string question, string subjectid,  CancellationToken ct = default);
        Task<string> GenerateSurveyResultAsync(UserLearningTargetDto target, UserLearningBehaviorDto behavior, CancellationToken ct = default);
        Task<string> GenerateBehaviorResultAsync(UserLearningBehaviorDto behavior, CancellationToken ct = default);
        Task<string> GenerateQuizQuestionsAsync(object roadmap, object roadmapnode, int questionCount, CancellationToken ct = default);
    }
}
