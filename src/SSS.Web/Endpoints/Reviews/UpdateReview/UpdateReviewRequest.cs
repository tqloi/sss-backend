namespace SSS.Web.Endpoints.Reviews.UpdateReview
{
    public class UpdateReviewRequest
    {
        public long Id { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
    }
}
