using FastEndpoints;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Entities.Learning;

namespace SSS.Web.Endpoints.Test.SurveyAnalysis.AnalyzeBehavior
{
    public class AnalyzeBehaviorEndpoint(ISurveyAnalysisService analysisService)
        : EndpointWithoutRequest<UserLearningBehavior>
    {
        public override void Configure()
        {
            Post("/test/survey-analysis/behavior/{responseId}");
            AllowAnonymous();
            Description(d => d.WithTags("Test"));
            Summary(s =>
            {
                s.Summary = "Test endpoint for analyzing learning behavior survey";
                s.Description = "Analyzes a LEARNING_BEHAVIOR survey response using AI and returns the result.";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var responseId = Route<long>("responseId");

            var result = await analysisService.AnalyzeBehaviorAsync(responseId, ct);

            await SendOkAsync(result, ct);
        }
    }
}
