using System.Text.Json.Serialization;

namespace SSS.Application.Features.QuizAttempts.Common
{
    public sealed class SubmitQuizAttempDto
    {
        public long Id { get; set; }

        public List<SubmitQuizAttemptAnswerDto> Answers { get; set; } = new List<SubmitQuizAttemptAnswerDto>();
    }
}
