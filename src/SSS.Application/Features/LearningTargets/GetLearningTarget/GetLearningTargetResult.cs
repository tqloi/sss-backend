namespace SSS.Application.Features.LearningTargets.GetLearningTarget
{
    public class GetLearningTargetResult
    {
        public string TargetRole { get; set; } = default!;
        public string CurrentLevel { get; set; } = default!;
        public int? TargetDeadlineMonths { get; set; }
    }
}
