namespace SSS.Application.Features.UserProfile.GetSurveyStatus;

public class GetSurveyStatusResponse
{
    public bool RequiresInitialSurvey { get; set; }
    public long? SurveyId { get; set; }
    public string? SurveyCode { get; set; }
    public string? RedirectUrl { get; set; }
}
