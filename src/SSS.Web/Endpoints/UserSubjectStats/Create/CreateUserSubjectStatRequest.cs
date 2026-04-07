namespace SSS.WebApi.Endpoints.UserSubjectStats.Create;

public sealed class CreateUserSubjectStatRequest
{
    public long SubjectId { get; set; }
    public decimal? ProficiencyLevel { get; set; }
    public decimal? TotalHoursSpent { get; set; }
    public string? WeakNodeIds { get; set; }
}
