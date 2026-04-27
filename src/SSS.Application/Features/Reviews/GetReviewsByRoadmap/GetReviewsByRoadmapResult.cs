using SSS.Application.Common.Dtos;
using SSS.Application.Features.Reviews.Common;

namespace SSS.Application.Features.Reviews.GetReviewsByRoadmap
{
    public class GetReviewsByRoadmapResult
    {
        public bool Success { get; set; }
        public PaginatedResponse<ReviewDto> Data { get; set; } = null!;
    }
}
