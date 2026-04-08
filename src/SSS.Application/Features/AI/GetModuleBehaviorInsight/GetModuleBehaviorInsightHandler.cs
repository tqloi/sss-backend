using MediatR;
using SSS.Application.Abstractions.External.AI.PipeLine;

namespace SSS.Application.Features.AI.GetModuleBehaviorInsight
{
    public sealed class GetModuleBehaviorInsightHandler(IPipeLine pipeLine)
        : IRequestHandler<GetModuleBehaviorInsightQuery, GetModuleBehaviorInsightResult>
    {
        public async Task<GetModuleBehaviorInsightResult> Handle(GetModuleBehaviorInsightQuery request, CancellationToken ct)
        {
            var context = await pipeLine.BuildStudyPlanContextAsync(request.UserId, request.StudyPlanId.ToString(), ct);

            if (string.IsNullOrWhiteSpace(context))
            {
                return new GetModuleBehaviorInsightResult
                {
                    Success = false,
                    Message = "No learning context data found.",
                    Insight = null
                };
            }

            var insight = await pipeLine.GenerateModuleBehaviorInsightAsync(context, ct);

            if (string.IsNullOrWhiteSpace(insight))
            {
                return new GetModuleBehaviorInsightResult
                {
                    Success = false,
                    Message = "Failed to generate insight.",
                    Insight = null
                };
            }

            return new GetModuleBehaviorInsightResult
            {
                Success = true,
                Message = "Insight generated successfully.",
                Insight = insight
            };
        }
    }
}
