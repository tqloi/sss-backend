using SSS.Domain.Entities.Assessment;
using System.Text.Json;

namespace SSS.Application.Features.QuizAnswers.Common
{
    /// <summary>
    /// Unified helper for extracting selected option IDs from QuizAnswer entities.
    /// Provides consistent logic for both save and retrieve operations.
    /// </summary>
    public static class QuizAnswerExtractionHelper
    {
        /// <summary>
        /// Extracts all selected option IDs from a QuizAnswer entity.
        /// Checks TextValue for JSON array first (for multiple selections),
        /// then falls back to OptionId if no JSON found.
        /// </summary>
        public static List<long> ExtractSelectedOptionIds(QuizAnswer answer)
        {
            if (answer == null)
                return new List<long>();

            // First, try to parse TextValue as JSON (for multi-select)
            var optionIdsFromJson = ParseOptionIdsFromJson(answer.TextValue);
            if (optionIdsFromJson.Count > 0)
            {
                return optionIdsFromJson;
            }

            // Fallback to OptionId field (for single-select)
            if (answer.OptionId.HasValue && answer.OptionId.Value > 0)
            {
                return new List<long> { answer.OptionId.Value };
            }

            return new List<long>();
        }

        /// <summary>
        /// Normalizes selected option IDs from a SaveQuizAnswerByQuestionDto.
        /// Merges both OptionIds array and legacy OptionId field.
        /// </summary>
        public static List<long> NormalizeIncomingOptionIds(SaveQuizAnswerByQuestionDto dto)
        {
            var selectedOptionIds = new List<long>();

            // Add from OptionIds array
            if (dto.OptionIds.Count > 0)
            {
                selectedOptionIds.AddRange(dto.OptionIds.Where(optionId => optionId > 0));
            }

            // Add from legacy OptionId field if not already included
            if (dto.OptionId.HasValue && dto.OptionId.Value > 0)
            {
                selectedOptionIds.Add(dto.OptionId.Value);
            }

            return selectedOptionIds.Distinct().ToList();
        }

        /// <summary>
        /// Parses option IDs from JSON-serialized TextValue.
        /// Returns empty list if TextValue is null, not JSON, or contains no valid IDs.
        /// </summary>
        private static List<long> ParseOptionIdsFromJson(string? textValue)
        {
            if (string.IsNullOrWhiteSpace(textValue))
            {
                return new List<long>();
            }

            try
            {
                var parsed = JsonSerializer.Deserialize<List<long>>(textValue);
                return parsed?.Where(id => id > 0).ToList() ?? new List<long>();
            }
            catch (JsonException)
            {
                return new List<long>();
            }
        }
    }
}
