using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.AI.CreateAiQuizQuestions
{
    public sealed class CreateAiQuizQuestionsCommand : IRequest<CreateAiQuizQuestionsResult>
    {
        public long QuizId { get; set; }
        public long RoadmapId { get; set; }
        public long RoadmapNodeId { get; set; }
        public string Level { get; set; } = null!;
        public int QuestionCount { get; set; } = 5;
    }
}
