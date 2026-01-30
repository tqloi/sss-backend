using MediatR;

namespace SSS.Application.Features.UserProfile.GetSurveyStatus;

public class GetSurveyStatusQuery : IRequest<GetSurveyStatusResponse>
{
    public string UserId { get; set; } = default!;
}
