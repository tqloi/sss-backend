namespace SSS.WebApi.Endpoints.UserSubjectStats.Update;

public sealed class UpdateUserSubjectStatRequest
{
    public long Id { get; set; }
    public decimal? ProficiencyLevel { get; set; }
    public decimal? TotalHoursSpent { get; set; }
    public string? WeakNodeIds { get; set; }
}
