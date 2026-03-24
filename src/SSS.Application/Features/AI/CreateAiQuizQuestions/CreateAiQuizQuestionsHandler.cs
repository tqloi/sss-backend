using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizQuestions.CreateQuizQuestions;
using SSS.Domain.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.AI.CreateAiQuizQuestions
{
    public sealed class CreateAiQuizQuestionsHandler(
        IAppDbContext dbContext,
        IPipeLine pipeLine)
        : IRequestHandler<CreateAiQuizQuestionsCommand, CreateAiQuizQuestionsResult>
    {
        public async Task<CreateAiQuizQuestionsResult> Handle(CreateAiQuizQuestionsCommand request, CancellationToken cancellationToken)
        {
            var roadmap = await dbContext.Roadmaps
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.RoadmapId, cancellationToken)
                ?? throw new InvalidOperationException($"Roadmap {request.RoadmapId} not found");

            var roadmapNode = await dbContext.RoadmapNodes
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == request.RoadmapNodeId, cancellationToken)
                ?? throw new InvalidOperationException($"RoadmapNode {request.RoadmapNodeId} not found");

            if (roadmapNode.RoadmapId != request.RoadmapId)
            {
                throw new InvalidOperationException("Roadmap node does not belong to the provided roadmap.");
            }

            var quiz = await dbContext.Quizzes
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken)
                ?? throw new InvalidOperationException($"Quiz {request.QuizId} not found");

            if (quiz.RoadmapNodeId != request.RoadmapNodeId)
            {
                throw new InvalidOperationException("Quiz does not belong to the provided roadmap node.");
            }

            var level = request.Level.Trim();

            var roadmapJson = JsonSerializer.Serialize(new
            {
                roadmap.Id,
                roadmap.Title,
                roadmap.Description
            });

            var roadmapNodeJson = JsonSerializer.Serialize(new
            {
                roadmapNode.Id,
                roadmapNode.RoadmapId,
                roadmapNode.Title,
                roadmapNode.Description,
                roadmapNode.Difficulty,
                roadmapNode.OrderNo
            });

            var aiResponse = await pipeLine.GenerateQuizQuestionsAsync(
                roadmapJson,
                roadmapNodeJson,
                level,
                request.QuestionCount,
                cancellationToken);

            aiResponse = aiResponse
                .Replace("```json", string.Empty)
                .Replace("```", string.Empty)
                .Trim();

            var deserializeOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            deserializeOptions.Converters.Add(new JsonStringEnumConverter());

            var aiQuestions = JsonSerializer.Deserialize<List<AiGeneratedQuestion>>(aiResponse, deserializeOptions)
                ?? throw new InvalidOperationException("AI response could not be parsed.");

            var previewQuestions = aiQuestions.Select((q, index) => new CreateQuizQuestionWithOptionsDto
            {
                QuizId = quiz.Id,
                Level = level,
                QuestionKey = string.IsNullOrWhiteSpace(q.QuestionKey)
                    ? $"QQ_{Guid.NewGuid():N}"[..11]
                    : q.QuestionKey,
                Prompt = q.Prompt,
                Type = q.Type,
                ScoreWeight = q.ScoreWeight,
                OrderNo = q.OrderNo <= 0 ? index + 1 : q.OrderNo,
                IsRequired = q.IsRequired,
                Options = q.Options.Select((o, optionIndex) => new CreateQuizQuestionOptionInputDto
                {
                    ValueKey = string.IsNullOrWhiteSpace(o.ValueKey) ? $"OPT_{optionIndex + 1}" : o.ValueKey,
                    DisplayText = o.DisplayText,
                    IsCorrect = o.IsCorrect,
                    ScoreValue = o.ScoreValue,
                    OrderNo = o.OrderNo <= 0 ? optionIndex + 1 : o.OrderNo
                }).ToList()
            }).ToList();

            return new CreateAiQuizQuestionsResult
            {
                Success = true,
                Message = "AI quiz questions generated successfully.",
                Questions = previewQuestions
            };
        }

        private sealed class AiGeneratedQuestion
        {
            public string QuestionKey { get; set; } = string.Empty;
            public string Prompt { get; set; } = string.Empty;
            public QuizQuestionType Type { get; set; }
            public decimal ScoreWeight { get; set; }
            public int OrderNo { get; set; }
            public bool IsRequired { get; set; }
            public List<AiGeneratedOption> Options { get; set; } = new();
        }

        private sealed class AiGeneratedOption
        {
            public string ValueKey { get; set; } = string.Empty;
            public string DisplayText { get; set; } = string.Empty;
            public bool IsCorrect { get; set; }
            public decimal? ScoreValue { get; set; }
            public int OrderNo { get; set; }
        }
    }
}
