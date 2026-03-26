namespace SSS.Application.Features.QuizAttempts.Common
{
    public sealed class QuizBasicInfoDto
    {
        public long QuizId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Level { get; set; } = null!;
        public decimal? TotalScore { get; set; }
        public decimal PassingScore { get; set; }
    }
}