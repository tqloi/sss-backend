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

namespace SSS.Application.Features.Surveys.SurveyResponses.GetResponsesOfSurvey
{
    public class GetResponseOfSurveyHandler(IAppDbContext db, IMapper mapper) : IRequestHandler<GetResponseOfSurveyQuery, GetResponseOfSurveyResult>
    {
        public async Task<GetResponseOfSurveyResult> Handle(GetResponseOfSurveyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var surveyExists = await db.Surveys
                    .AnyAsync(c => c.Id == request.surveyId, cancellationToken);

                if (!surveyExists)
                {
                    return new GetResponseOfSurveyResult(false, $"Survey with ID {request.surveyId} not found");
                }

                var query = db.SurveyResponses
                    .Where(a => a.SurveyId == request.surveyId)
                    .AsQueryable();

               

                var projectedQuery = query
                    .OrderBy(q => q.Id)
                    .ProjectTo<SurveyResponseDto>(mapper.ConfigurationProvider);

                var paginatedResult = await PaginatedList<SurveyResponseDto>
                    .CreateAsync(
                       projectedQuery,
                       request.PageIndex,
                       request.PageSize,
                       cancellationToken
                   );

                return new GetResponseOfSurveyResult(true, "Responses retrieved successfully", paginatedResult);
            }
            catch (Exception ex)
            {
                return new GetResponseOfSurveyResult(false, $"Error get response by survey: {ex.Message}");
            }
        }
    }
}

