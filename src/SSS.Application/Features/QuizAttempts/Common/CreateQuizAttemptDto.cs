using System.Text.Json.Serialization;

namespace SSS.Application.Features.QuizAttempts.Common
{
    public class CreateQuizAttemptDto
    {
        public long StudyPlanModuleId { get; set; }

        public string? Level { get; set; }

        [JsonIgnore]
        public string UserId { get; set; } = null!;
    }
}
