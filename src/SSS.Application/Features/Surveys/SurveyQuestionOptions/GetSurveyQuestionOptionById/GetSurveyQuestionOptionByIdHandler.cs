using AutoMapper;
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

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionById
{
    public class GetSurveyQuestionOptionByIdHandler(IAppDbContext db, IMapper mapper) : IRequestHandler<GetSurveyQuestionOptionByIdQuery, GetSurveyQuestionOptionByIdResult>
    {
        public async Task<GetSurveyQuestionOptionByIdResult> Handle(GetSurveyQuestionOptionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var option = await db.SurveyQuestionOptions.AsNoTracking().FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (option == null)
                {
                    return new GetSurveyQuestionOptionByIdResult(false, "Option Not Found");
                }

                var dto = mapper.Map<SurveyQuestionOptionDto>(option);

                return new GetSurveyQuestionOptionByIdResult(true, "Success", dto);
            }
            catch (Exception ex)
            {
                return new GetSurveyQuestionOptionByIdResult(false, $"Error retrieving option: {ex.Message}");

            }
        }
    }
}
