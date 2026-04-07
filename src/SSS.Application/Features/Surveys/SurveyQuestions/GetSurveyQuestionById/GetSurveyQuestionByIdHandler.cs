using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById
{
    public class GetSurveyQuestionByIdHandler(IAppDbContext db, IMapper mapper) : IRequestHandler<GetSurveyQuestionByIdQuery, GetSurveyQuestionByIdResult>
    {
        public async Task<GetSurveyQuestionByIdResult> Handle(GetSurveyQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await db.SurveyQuestions.AsNoTracking().FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (question == null)
                {
                    return new GetSurveyQuestionByIdResult(false, "Question Not Found");
                }

                var dto = mapper.Map<SurveyQuestionDto>(question);
                
                return new GetSurveyQuestionByIdResult(true, "Success", dto);
            }
            catch (Exception ex)
            {
                return new GetSurveyQuestionByIdResult(false, $"Error retrieving question: {ex.Message}");

            }
        }
    }
}
