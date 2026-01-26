using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey
{
    public class GetQuestionsBySurveyHandler(IAppDbContext db, IMapper mapper) : IRequestHandler<GetQuestionsBySurveyQuery, GetQuestionsBySurveyResult>
    {
        public async Task<GetQuestionsBySurveyResult> Handle(GetQuestionsBySurveyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var surveyExists = await db.Surveys
                    .AnyAsync(c => c.Id == request.surveyId, cancellationToken);

                if (!surveyExists)
                {
                    return new GetQuestionsBySurveyResult(false, $"Survey with ID {request.surveyId} not found");
                }

                var query = db.SurveyQuestions
                    .Where(a => a.SurveyId == request.surveyId)
                    .AsQueryable();

                // Lọc theo SearchWord nếu có
                //if (!string.IsNullOrWhiteSpace(request.SearchWord))
                //{
                //    var searchTerm = request.SearchWord.Trim().ToLower();
                //    query = query.Where(a =>
                //        a.Prompt.ToLower().Contains(searchTerm) ||
                //        a.QuestionKey.ToLower().Contains(searchTerm)
                //    );
                //}

                var projectedQuery = query
                    .OrderBy(q => q.OrderNo)
                    .ProjectTo<SurveyQuestionDto>(mapper.ConfigurationProvider);

                var paginatedResult = await PaginatedList<SurveyQuestionDto>
                    .CreateAsync(
                       projectedQuery,
                       request.PageIndex,
                       request.PageSize,
                       cancellationToken
                   );

                return new GetQuestionsBySurveyResult(true, "Questions retrieved successfully", paginatedResult);
            }
            catch(Exception ex) 
            {
                return new GetQuestionsBySurveyResult(false, $"Error get question by survey: {ex.Message}");
            }
        }
    }
}
