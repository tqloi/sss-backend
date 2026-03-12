using FastEndpoints;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Entities.Learning;

namespace SSS.Web.Endpoints.Test.SurveyAnalysis.AnalyzeTarget
{
    public class AnalyzeTargetEndpoint(ISurveyAnalysisService analysisService)
        : EndpointWithoutRequest<UserLearningTarget>
    {
        public override void Configure()
        {
            Post("/test/survey-analysis/target/{responseId}");
            AllowAnonymous();
            Description(d => d.WithTags("Test"));
            Summary(s =>
            {
                s.Summary = "Test endpoint for analyzing learning target survey";
                s.Description = "Analyzes a ROADMAP_LEARNING_TARGET survey response using AI and returns the result.";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var responseId = Route<long>("responseId");

            var result = await analysisService.AnalyzeTargetAsync(responseId, ct);

            await SendOkAsync(result, ct);
        }
    }
}
