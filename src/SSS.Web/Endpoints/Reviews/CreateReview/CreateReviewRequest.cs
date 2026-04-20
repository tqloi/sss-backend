namespace SSS.Web.Endpoints.Reviews.CreateReview
{
    public class CreateReviewRequest
    {
        public long RoadmapId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
    }
}
