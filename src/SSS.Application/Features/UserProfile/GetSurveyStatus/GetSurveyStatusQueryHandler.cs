using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Constants;

namespace SSS.Application.Features.UserProfile.GetSurveyStatus;

public class GetSurveyStatusQueryHandler : IRequestHandler<GetSurveyStatusQuery, GetSurveyStatusResponse>
{
    private readonly IAppDbContext _context;

    public GetSurveyStatusQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetSurveyStatusResponse> Handle(GetSurveyStatusQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        // Find the LEARNING_BEHAVIOR survey (initial survey)
        var initialSurvey = await _context.Surveys
            .FirstOrDefaultAsync(s => s.Code == SurveyCodes.LearningBehavior, cancellationToken);

        if (initialSurvey == null)
        {
            // Survey not found, don't require it
            return new GetSurveyStatusResponse
            {
                RequiresInitialSurvey = false
            };
        }

        // Check if user has completed this survey
        var hasCompletedSurvey = await _context.SurveyResponses
            .AnyAsync(sr =>
                sr.UserId == userId &&
                sr.SurveyId == initialSurvey.Id &&
                sr.SubmittedAt != null,
                cancellationToken);

        if (hasCompletedSurvey)
        {
            return new GetSurveyStatusResponse
            {
                RequiresInitialSurvey = false
            };
        }

        // User needs to complete initial survey
        return new GetSurveyStatusResponse
        {
            RequiresInitialSurvey = true,
            SurveyId = initialSurvey.Id,
            SurveyCode = initialSurvey.Code,
            RedirectUrl = $"/surveys/{initialSurvey.Id}"
        };
    }
}
