using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Constants;
using SSS.Domain.Entities.Learning;
using SSS.Domain.Enums;
using System.Text.Json;

namespace SSS.Infrastructure.Services
{
    public class SurveyAnalysisService : ISurveyAnalysisService
    {
        private readonly IAppDbContext _context;
        private readonly ILlmRouter _llmRouter;
        private readonly ILogger<SurveyAnalysisService> _logger;

        public SurveyAnalysisService(
            IAppDbContext context,
            ILlmRouter llmRouter,
            ILogger<SurveyAnalysisService> logger)
        {
            _context = context;
            _llmRouter = llmRouter;
            _logger = logger;
        }

        public async Task<UserLearningBehavior> AnalyzeBehaviorAsync(long responseId, CancellationToken ct = default)
        {
            _logger.LogInformation("Analyzing behavior survey response {ResponseId}", responseId);

            // Load survey response with answers
            var response = await _context.SurveyResponses
                .Include(r => r.Survey)
                .Include(r => r.Answers).ThenInclude(a => a.Option)
                .Include(r => r.Answers).ThenInclude(a => a.Question).ThenInclude(q => q.Semantics)
                .FirstOrDefaultAsync(r => r.Id == responseId, ct);

            if (response == null)
                throw new InvalidOperationException($"Survey response {responseId} not found");

            if (response.Survey.Code != SurveyCodes.LearningBehavior)
                throw new InvalidOperationException($"Response {responseId} is not a LEARNING_BEHAVIOR survey");

            // Map answers using semantic fields
            var mappedData = MapSurveyAnswers(response);

            // Build structured input for AI
            var (systemPrompt, userPrompt) = BuildBehaviorAnalysisPrompt(mappedData);

            // Debug: log the full survey data sent to AI so we can verify numeric values
            var surveyDataDebugJson = System.Text.Json.JsonSerializer.Serialize(mappedData,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = false, PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
            _logger.LogWarning("Survey data sent to AI: {SurveyData}", surveyDataDebugJson);

            // Call AI service to analyze
            _logger.LogDebug("Calling AI for behavior analysis using LlmTask.SurveyAnalysis");
            var llmProvider = _llmRouter.Resolve(LlmTask.GenerateRoadmap);
            var aiResult = await llmProvider.AskAsync(systemPrompt, userPrompt, ct);

            // Parse AI result and build UserLearningBehavior object
            var behavior = ParseBehaviorResult(aiResult, response.UserId, responseId);

            _logger.LogInformation("Behavior analysis completed for response {ResponseId}", responseId);

            return behavior;
        }

        public async Task<UserLearningTarget> AnalyzeTargetAsync(long responseId, CancellationToken ct = default)
        {
            _logger.LogInformation("Analyzing target survey response {ResponseId}", responseId);

            // Load survey response with answers
            var response = await _context.SurveyResponses
                .Include(r => r.Survey)
                .Include(r => r.Answers).ThenInclude(a => a.Option)
                .Include(r => r.Answers).ThenInclude(a => a.Question).ThenInclude(q => q.Semantics)
                .FirstOrDefaultAsync(r => r.Id == responseId, ct);

            if (response == null)
                throw new InvalidOperationException($"Survey response {responseId} not found");

            if (response.Survey.Code != SurveyCodes.RoadmapLearningTarget)
                throw new InvalidOperationException($"Response {responseId} is not a ROADMAP_LEARNING_TARGET survey");

            // Map answers using semantic fields
            var mappedData = MapSurveyAnswers(response);

            // Build structured input for AI
            var (systemPrompt, userPrompt) = BuildTargetAnalysisPrompt(mappedData);

            // Call AI service to analyze
            _logger.LogDebug("Calling AI for target analysis using LlmTask.SurveyAnalysis");
            var llmProvider = _llmRouter.Resolve(LlmTask.GenerateRoadmap);
            var aiResult = await llmProvider.AskAsync(systemPrompt, userPrompt, ct);

            // Parse AI result and build UserLearningTarget object
            var target = ParseTargetResult(aiResult, response.UserId, responseId);

            _logger.LogInformation("Target analysis completed for response {ResponseId}", responseId);

            return target;
        }

        #region Private Helpers

        // Carries one survey dimension's data enriched with semantic context for the AI
        private record SurveyDimension(
            string DimensionCode,
            string Evaluates,
            string? AiHint,
            double? Weight,
            List<object?> Values
        );

        private List<SurveyDimension> MapSurveyAnswers(SSS.Domain.Entities.Assessment.SurveyResponse response)
        {
            // Key = (DimensionCode, Evaluates) — prevents collision when multiple questions
            // share the same DimensionCode (e.g. visual/reading/practice all under "learning_style")
            var dimDict = new Dictionary<(string DimensionCode, string Evaluates), (string? AiHint, double? Weight, List<object?> Values)>();

            foreach (var answer in response.Answers)
            {
                var rawValue = ExtractAnswerValue(answer);

                if (answer.Question.Semantics.Any())
                {
                    foreach (var semantic in answer.Question.Semantics)
                    {
                        var key = (semantic.DimensionCode, semantic.Evaluates);

                        if (!dimDict.TryGetValue(key, out var dim))
                        {
                            dim = (semantic.AIHint, semantic.Weight, new List<object?>());
                            dimDict[key] = dim;
                        }

                        dim.Values.Add(rawValue);
                    }
                }
                else
                {
                    // fallback nếu không có semantic
                    var dimension = answer.Question.QuestionKey ?? "unknown_dimension";

                    var key = ("auto_inferred", dimension);

                    if (!dimDict.TryGetValue(key, out var dim))
                    {
                        dim = (
                            $"AI should infer meaning from question: {answer.Question.Prompt}",
                            1.0,
                            new List<object?>()
                        );

                        dimDict[key] = dim;
                    }

                    dim.Values.Add(rawValue);
                }
            }

            return dimDict.Select(kvp => new SurveyDimension(
                kvp.Key.DimensionCode,
                kvp.Key.Evaluates,
                kvp.Value.AiHint,
                kvp.Value.Weight,
                kvp.Value.Values
            )).ToList();
        }

        private object? ExtractAnswerValue(SSS.Domain.Entities.Assessment.SurveyAnswer answer)
        {
            // numeric/scale questions should use NumberValue first
            if (answer.NumberValue.HasValue)
                return answer.NumberValue.Value;

            // for choice questions
            if (answer.Option != null)
                return answer.Option.ValueKey;

            // free text
            if (!string.IsNullOrWhiteSpace(answer.TextValue))
                return answer.TextValue;

            return null;
        }
        /// <summary>
        /// Pre-computes normalized learning style weights in C# (not delegated to AI).
        /// </summary>
        private (decimal wVisual, decimal wReading, decimal wPractice) ComputeLearningStyleWeights(List<SurveyDimension> dimensions)
        {
            static decimal GetScore(List<SurveyDimension> dims, string evaluates)
            {
                var dim = dims.FirstOrDefault(d =>
                    string.Equals(d.Evaluates, evaluates, StringComparison.OrdinalIgnoreCase));
                if (dim == null || dim.Values.Count == 0) return 0;
                var raw = dim.Values[0];
                return raw switch
                {
                    decimal d => d,
                    int i     => (decimal)i,
                    long l    => (decimal)l,
                    double db => (decimal)db,
                    float f   => (decimal)f,
                    _         => decimal.TryParse(raw?.ToString(), out var p) ? p : 0
                };
            }

            var v = GetScore(dimensions, "visual_preference");
            var r = GetScore(dimensions, "reading_preference");
            var p = GetScore(dimensions, "practice_preference");
            var sum = v + r + p;

            if (sum == 0) return (0.34m, 0.33m, 0.33m);

            return (
                Math.Round(v / sum, 2),
                Math.Round(r / sum, 2),
                Math.Round(p / sum, 2)
            );
        }

        /// <summary>
        /// Builds AI prompt for behavior analysis.
        /// Learning style weights are pre-computed in C#; AI only decodes symbolic fields.
        /// </summary>
        private (string systemPrompt, string userPrompt) BuildBehaviorAnalysisPrompt(List<SurveyDimension> dimensions)
        {
            // Compute weights deterministically — do NOT let AI do math
            var (wVisual, wReading, wPractice) = ComputeLearningStyleWeights(dimensions);

            // Only send non-numeric dimensions to AI (save tokens, avoid confusion)
            var symbolDimensions = dimensions
                .Where(d => d.Evaluates is not ("visual_preference" or "reading_preference" or "practice_preference"))
                .ToList();

            var surveyDataJson = JsonSerializer.Serialize(symbolDimensions, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var systemPrompt = @"You are an AI assistant that maps raw survey answers into a structured learning behavior profile.
Return ONLY a raw JSON object. Do NOT use markdown, code fences, or any extra text outside the JSON.";

            var userPrompt = $@"Map the following survey answers into the required JSON output.

Each entry contains:
- evaluates: the specific field it maps to
- values: the exact answer keys the user submitted

Survey Data:
{surveyDataJson}

=== FIELD DECODING RULES ===

[evaluates = ""available_days""]
  Map each value key to a full English day name:
  ""mon""→""Monday"", ""tue""→""Tuesday"", ""wed""→""Wednesday"", ""thu""→""Thursday"",
  ""fri""→""Friday"", ""sat""→""Saturday"", ""sun""→""Sunday""
  Output: string array of all selected days.

[evaluates = ""preferred_time_blocks""]
  Capitalize the first letter only:
  ""morning""→""Morning"", ""noon""→""Noon"", ""afternoon""→""Afternoon"", ""evening""→""Evening""
  Output: string array of all selected time blocks.

[evaluates = ""session_length_minutes""]
  Parse the string value as an integer:
  ""20""→20, ""40""→40, ""60""→60, ""90""→90
  Output: single integer.

[evaluates = ""self_discipline_level""]
  Map to EXACTLY one of these (case-sensitive):
  ""on_time"" → ""OnTime""
  ""late""    → ""LateButDone""
  ""drop""    → ""DropMidway""
  Output: single string.

[evaluates = ""common_difficulties""]
  Map each value key to a readable label:
  ""start""      → ""Getting started""
  ""focus""      → ""Maintaining focus""
  ""understand"" → ""Understanding content""
  ""apply""      → ""Applying knowledge""
  Output: string array of all selected difficulties.

=== PRE-COMPUTED VALUES (use these verbatim, do NOT recalculate) ===
  weight_visual   = {wVisual:F2}
  weight_reading  = {wReading:F2}
  weight_practice = {wPractice:F2}

=== OUTPUT FORMAT ===
Return ONLY this JSON (no markdown, no extra text):
{{
  ""available_days"": [""Monday"", ""Wednesday"", ""Friday""],
  ""preferred_time_blocks"": [""Morning"", ""Evening""],
  ""session_length_minutes"": 90,
  ""weight_visual"": {wVisual:F2},
  ""weight_reading"": {wReading:F2},
  ""weight_practice"": {wPractice:F2},
  ""discipline_type"": ""OnTime"",
  ""common_difficulties"": [""Getting started"", ""Maintaining focus""]
}}";

            return (systemPrompt, userPrompt);
        }

        /// <summary>
        /// Builds AI prompt for target analysis
        /// </summary>
        private (string systemPrompt, string userPrompt) BuildTargetAnalysisPrompt(List<SurveyDimension> dimensions)
        {
            var surveyDataJson = JsonSerializer.Serialize(dimensions, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var systemPrompt = @"You are an AI assistant that maps raw survey answers into a structured learning target profile.
Return ONLY a raw JSON object. Do NOT use markdown, code fences, or any extra text outside the JSON.";

            var userPrompt = $@"Below is the raw survey data for a learning roadmap target survey:

Survey Data:
{surveyDataJson}

=== FIELD DECODING RULES ===

[evaluates = ""current_skill_level""]
  ""beginner""     → ""Beginner""
  ""basic""        → ""Basic""
  ""intermediate"" → ""Intermediate""

[evaluates = ""target_deadline_months""]
  ""3_6""    → 6   (represents 3–6 months, use the upper bound)
  ""6_12""   → 12  (represents 6–12 months, use the upper bound)
  ""unknown"" → null

[evaluates = ""goal_description""]

    This field represents the user's learning goal in free text.

    Rules:
    - Use the user's original text ONLY if it expresses a clear and meaningful learning goal.
    - Trim whitespace.

    You MUST return null if the input is vague, meaningless, or does not indicate a clear goal.

    A goal is NOT meaningful if it contains uncertainty or lack of intent, such as:
    - ""i don't know"", ""idk"", ""no idea""
    - ""maybe"", ""not sure""
    - ""just exploring"", ""trying things""
    - ""anything"", ""whatever"", ""something""

    Important:
    - Mentioning topics (backend, AI, frontend) is NOT enough.
    - The user must clearly commit to a goal.

    Output:
    - meaningful → keep original text
    - not meaningful → null

[target_role]
    Extract a concise role name (e.g., ""Backend Developer"", ""Data Analyst"") ONLY if the user clearly commits to a specific goal.

    If the goal is uncertain or null → target_role MUST be null.

    Do NOT guess.
    Do NOT infer from weak signals.
    Be conservative.
    Be conservative. When uncertain, return null.

    Examples:

    Input: ""i dont know maybe backend""
    → target_role: null

    Input: ""i want to become a backend developer""
    → target_role: ""Backend Developer""

=== OUTPUT FORMAT ===
Return ONLY this JSON (no markdown, no commentary):
{{
  ""current_level"": ""Beginner"",
  ""deadline_months"": 6,
  ""goal_description"": ""I want to become a backend developer""
  ""target_role"": ""Backend Developer""
}}";

            return (systemPrompt, userPrompt);
        }

        /// <summary>
        /// Strips markdown code block fences (```json ... ``` or ``` ... ```) from AI output
        /// </summary>
        private static string StripMarkdownCodeBlock(string text)
        {
            var s = text.Trim();
            if (s.StartsWith("```"))
            {
                var firstNewline = s.IndexOf('\n');
                if (firstNewline >= 0)
                    s = s[(firstNewline + 1)..];
                if (s.EndsWith("```"))
                    s = s[..^3];
                s = s.Trim();
            }
            return s;
        }

        /// <summary>
        /// Parses AI result into UserLearningBehavior object
        /// </summary>
        private UserLearningBehavior ParseBehaviorResult(string aiResult, string userId, long responseId)
        {
            // Parse AI JSON result (strip markdown fences if present)
            var jsonDoc = JsonDocument.Parse(StripMarkdownCodeBlock(aiResult));
            var root = jsonDoc.RootElement;

            return new UserLearningBehavior
            {
                UserId = userId,
                SourceSurveyResponseId = responseId,
                SnapshotAt = DateTime.UtcNow,
                SnapshotVersion = 1,

                // Time & availability
                AvailableDaysJson = GetJsonProperty(root, "available_days")?.GetRawText(),
                PreferredTimeBlocksJson = GetJsonProperty(root, "preferred_time_blocks")?.GetRawText(),
                SessionLengthPrefMinutes = GetIntProperty(root, "session_length_minutes"),

                // Learning style weights
                WVisual = GetDecimalProperty(root, "weight_visual") ?? 0.33m,
                WReading = GetDecimalProperty(root, "weight_reading") ?? 0.33m,
                WPractice = GetDecimalProperty(root, "weight_practice") ?? 0.34m,

                // Discipline
                DisciplineType = ParseDisciplineType(GetStringProperty(root, "discipline_type")),
                CommonDifficultiesJson = GetJsonProperty(root, "common_difficulties")?.GetRawText(),

                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Parses AI result into UserLearningTarget object
        /// </summary>
        private UserLearningTarget ParseTargetResult(string aiResult, string userId, long responseId)
        {
            // Parse AI JSON result (strip markdown fences if present)
            var jsonDoc = JsonDocument.Parse(StripMarkdownCodeBlock(aiResult));
            var root = jsonDoc.RootElement;

            var aiGoal = GetStringProperty(root, "goal_description");
            var aiRole = GetStringProperty(root, "target_role");

            // target role ưu tiên AI extract
            string targetRole;

            if (!string.IsNullOrWhiteSpace(aiRole))
            {
                targetRole = aiRole.Trim();
            }
            else if (!string.IsNullOrWhiteSpace(aiGoal))
            {
                // fallback: dùng raw goal
                targetRole = aiGoal.Trim();
            }
            else
            {
                targetRole = "Learner";
            }

            return new UserLearningTarget
            {
                UserId = userId,
                SourceSurveyResponseId = responseId,
                SnapshotAt = DateTime.UtcNow,
                ProfileVersion = 1,

                // target_role and roadmap_id are not captured by this survey;
                // the caller must set RoadmapId after receiving the result.
                TargetRole = targetRole,
                RoadmapId = 0,

                CurrentLevel = GetStringProperty(root, "current_level") ?? "Beginner",
                TargetDeadlineMonths = GetIntProperty(root, "deadline_months"),
                GoalDescription = aiGoal,

                Status = TargetStatus.active,
                CreatedAt = DateTime.UtcNow
            };
        }

        // JSON helper methods
        private JsonElement? GetJsonProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out var property) ? property : null;
        }

        private string? GetStringProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
                ? property.GetString()
                : null;
        }

        private int? GetIntProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Number)
                return property.GetInt32();
            return null;
        }

        private long? GetLongProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Number)
                return property.GetInt64();
            return null;
        }

        private decimal? GetDecimalProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Number)
                return property.GetDecimal();
            return null;
        }

        private DisciplineType? ParseDisciplineType(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value.ToLowerInvariant() switch
            {
                "ontime" => DisciplineType.OnTime,
                "on time" => DisciplineType.OnTime,
                "latebutdone" => DisciplineType.LateButDone,
                "late but done" => DisciplineType.LateButDone,
                "dropmidway" => DisciplineType.DropMidway,
                "drop midway" => DisciplineType.DropMidway,
                _ => null
            };
        }

        #endregion
    }
}
