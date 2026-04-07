using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.AI.CreateAiRoadMap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateAiTaskItems
{
    public sealed class CreateAiTaskItemsHandler(IPipeLine pipeLine, IAppDbContext dbContext) 
        : IRequestHandler<CreateAiTaskItemsCommand, CreateAiTaskItemsResult>
    {
        public async Task<CreateAiTaskItemsResult> Handle(CreateAiTaskItemsCommand request, CancellationToken cancellationToken)
        {
            var module = await dbContext.StudyPlanModules
                .AsNoTracking()
                .FirstOrDefaultAsync(spm => spm.Id == request.studyPlanModuleId)
                ?? throw new InvalidOperationException($"StudyPlanModule {request.studyPlanModuleId} not found");

            var roadmapnode = await dbContext.RoadmapNodes
                .AsNoTracking()
                .FirstOrDefaultAsync(rm => rm.Id == module.RoadmapNodeId)
                ?? throw new InvalidOperationException($"RoadmapNode {module.RoadmapNodeId} not found");

            var roadmap = await dbContext.Roadmaps
                .AsNoTracking()
                .FirstOrDefaultAsync(rm => rm.Id == roadmapnode.RoadmapId)
                ?? throw new InvalidOperationException($"Roadmap {roadmapnode.RoadmapId} not found");

            var roadmapJson = JsonSerializer.Serialize(roadmap);
            var roadmapNodeJson = JsonSerializer.Serialize(roadmapnode);

            var aiResponse = await pipeLine.GenerateStudyPlanAsync(request.UserId, module.StudyPlanId.ToString(), roadmapJson, roadmapNodeJson, cancellationToken);

            if (aiResponse == null) throw new NotImplementedException();

            aiResponse = aiResponse
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            var normalizedJson = NormalizeScheduledDates(aiResponse);

            return new CreateAiTaskItemsResult
            {
                Success = true,
                Message = "AI task generated successfully",
                RawTaskItens = normalizedJson
            };
        }

        private static JsonElement NormalizeScheduledDates(string aiResponse)
        {
            var node = JsonNode.Parse(aiResponse)
                ?? throw new JsonException("AI response could not be parsed into JSON.");

            NormalizeNode(node);

            using var normalizedDocument = JsonDocument.Parse(node.ToJsonString());
            return normalizedDocument.RootElement.Clone();
        }

        private static void NormalizeNode(JsonNode? node)
        {
            if (node is JsonObject obj)
            {
                if (obj.TryGetPropertyValue("scheduledDate", out var scheduledDateNode) &&
                    scheduledDateNode is JsonValue scheduledDateValue &&
                    scheduledDateValue.TryGetValue<string>(out var scheduledDateText) &&
                    !string.IsNullOrWhiteSpace(scheduledDateText))
                {
                    obj["scheduledDate"] = NormalizeToOneAmUtc(scheduledDateText);
                }

                foreach (var property in obj)
                {
                    NormalizeNode(property.Value);
                }

                return;
            }

            if (node is JsonArray array)
            {
                foreach (var item in array)
                {
                    NormalizeNode(item);
                }
            }
        }

        private static string NormalizeToOneAmUtc(string scheduledDateText)
        {
            if (DateTimeOffset.TryParse(
                    scheduledDateText,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out var parsedDate))
            {
                var normalized = new DateTimeOffset(
                    parsedDate.UtcDateTime.Year,
                    parsedDate.UtcDateTime.Month,
                    parsedDate.UtcDateTime.Day,
                    1,
                    0,
                    0,
                    TimeSpan.Zero);

                return normalized.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
            }

            return scheduledDateText;
        }
    }
}
