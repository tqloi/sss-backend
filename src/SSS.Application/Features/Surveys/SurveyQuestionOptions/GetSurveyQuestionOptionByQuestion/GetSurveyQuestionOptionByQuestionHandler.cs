using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionByQuestion
{
    public class GetSurveyQuestionOptionByQuestionHandler(IAppDbContext db, IMapper mapper) : IRequestHandler<GetSurveyQuestionOptionByQuestionQuery, GetSurveyQuestionOptionByQuestionResult>
    {
        public async Task<GetSurveyQuestionOptionByQuestionResult> Handle(GetSurveyQuestionOptionByQuestionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var questionExists = await db.SurveyQuestions
                    .AnyAsync(c => c.Id == request.QuestionId, cancellationToken);

                if (!questionExists)
                {
                    return new GetSurveyQuestionOptionByQuestionResult(false, $"Question with ID {request.QuestionId} not found");
                }

                var query = db.SurveyQuestionOptions
                    .Where(a => a.QuestionId == request.QuestionId)
                    .AsQueryable();

               
                var projectedQuery = query
                    .OrderBy(q => q.OrderNo)
                    .ProjectTo<SurveyQuestionOptionDto>(mapper.ConfigurationProvider);

                var paginatedResult = await PaginatedList<SurveyQuestionOptionDto>
                    .CreateAsync(
                       projectedQuery,
                       request.PageIndex,
                       request.PageSize,
                       cancellationToken
                   );

                return new GetSurveyQuestionOptionByQuestionResult(true, "Options retrieved successfully", paginatedResult);
            }
            catch (Exception ex)
            {
                return new GetSurveyQuestionOptionByQuestionResult(false, $"Error get options by question: {ex.Message}");
            }
        }
    }
}

