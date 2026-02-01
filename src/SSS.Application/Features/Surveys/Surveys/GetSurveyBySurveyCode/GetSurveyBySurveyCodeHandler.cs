using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.Surveys.GetSurveyBySurveyCode;

public class GetSurveyBySurveyCodeHandler(IAppDbContext db, IMapper mapper) 
    : IRequestHandler<GetSurveyBySurveyCodeQuery, GetSurveyBySurveyCodeResult>
{
    public async Task<GetSurveyBySurveyCodeResult> Handle(
        GetSurveyBySurveyCodeQuery request, 
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.SurveyCode))
            {
                return new GetSurveyBySurveyCodeResult(false, "Survey code is required");
            }

            var survey = await db.Surveys
                .Include(s => s.Questions.OrderBy(q => q.OrderNo))
                    .ThenInclude(q => q.Options.OrderBy(o => o.OrderNo))
                .FirstOrDefaultAsync(s => s.Code == request.SurveyCode, cancellationToken);

            if (survey == null)
            {
                return new GetSurveyBySurveyCodeResult(false, $"Survey with code '{request.SurveyCode}' not found");
            }

            var dto = mapper.Map<SurveyDto>(survey);

            return new GetSurveyBySurveyCodeResult(true, "Survey retrieved successfully", dto);
        }
        catch (Exception ex)
        {
            return new GetSurveyBySurveyCodeResult(false, $"Error retrieving survey: {ex.Message}");
        }
    }
}