using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.GetSurveyById
{
    public class GetSurveyByIdHandler(IAppDbContext db) : IRequestHandler<GetSurveyByIdQuery, GetSurveyByIdResult>
    {
        public async Task<GetSurveyByIdResult> Handle(GetSurveyByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var survey = await db.Surveys.AsNoTracking().FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (survey == null)
                {
                    return new GetSurveyByIdResult(false, "Survey Not Found");
                }

                var dto = new SurveyDto
                (
                    survey.Title,
                    survey.Code,
                    survey.Status

                );

                return new GetSurveyByIdResult(true, "Success", dto);
            }
            catch (Exception ex)
            {
                return new GetSurveyByIdResult(false, $"Error retrieving survey: {ex.Message}");

            }
        }

        
    }
}
