using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Constants;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey
{
    public class GetQuestionsBySurveyHandler(IAppDbContext db, IMapper mapper, ICacheService cacheService) : IRequestHandler<GetQuestionsBySurveyQuery, GetQuestionsBySurveyResult>
    {
        public async Task<GetQuestionsBySurveyResult> Handle(GetQuestionsBySurveyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pageIndex = request.PageIndex < 1 ? 1 : request.PageIndex;
                var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
                var cacheKey = $"survey:questions:{request.surveyId}";

                var cachedQuestions = await cacheService.GetAsync<List<SurveyQuestionDto>>(cacheKey);
                if (cachedQuestions != null)
                {
                    var cachedPagedItems = cachedQuestions
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var cachedPaginatedResult = new PaginatedList<SurveyQuestionDto>(
                        cachedPagedItems,
                        cachedQuestions.Count,
                        pageIndex,
                        pageSize
                    );

                    return new GetQuestionsBySurveyResult(true, "Questions retrieved successfully", cachedPaginatedResult);
                }

                var surveyExists = await db.Surveys
                    .AsNoTracking()
                    .AnyAsync(c => c.Id == request.surveyId, cancellationToken);

                if (!surveyExists)
                {
                    return new GetQuestionsBySurveyResult(false, $"Survey with ID {request.surveyId} not found");
                }

                // Lọc theo SearchWord nếu có
                //if (!string.IsNullOrWhiteSpace(request.SearchWord))
                //{
                //    var searchTerm = request.SearchWord.Trim().ToLower();
                //    query = query.Where(a =>
                //        a.Prompt.ToLower().Contains(searchTerm) ||
                //        a.QuestionKey.ToLower().Contains(searchTerm)
                //    );
                //}

                var questions = await db.SurveyQuestions
                    .AsNoTracking()
                    .Where(a => a.SurveyId == request.surveyId)
                    .OrderBy(q => q.OrderNo)
                    .ProjectTo<SurveyQuestionDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                await cacheService.SetAsync(cacheKey, questions, CacheConstants.DefaultExpiration);

                var pagedItems = questions
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var paginatedResult = new PaginatedList<SurveyQuestionDto>(
                    pagedItems,
                    questions.Count,
                    pageIndex,
                    pageSize
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
