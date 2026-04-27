namespace SSS.Application.Features.Reviews.Common
{
    public class ReviewDto
    {
        public long Id { get; set; }
        public long RoadmapId { get; set; }
        public string RoadmapTitle { get; set; } = null!;
        public string? ReviewerId { get; set; }
        public string? ReviewerName { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
