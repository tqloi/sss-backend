using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionByQuestion
{
    public class GetSurveyQuestionOptionByQuestionHandler(IAppDbContext db, IMapper mapper, ICacheService cacheService) : IRequestHandler<GetSurveyQuestionOptionByQuestionQuery, GetSurveyQuestionOptionByQuestionResult>
    {
        public async Task<GetSurveyQuestionOptionByQuestionResult> Handle(GetSurveyQuestionOptionByQuestionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pageIndex = request.PageIndex < 1 ? 1 : request.PageIndex;
                var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
                var cacheKey = $"survey:question-options:{request.QuestionId}";

                var cachedOptions = await cacheService.GetAsync<List<SurveyQuestionOptionDto>>(cacheKey);
                if (cachedOptions != null)
                {
                    var cachedPagedItems = cachedOptions
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var cachedPaginatedResult = new PaginatedList<SurveyQuestionOptionDto>(
                        cachedPagedItems,
                        cachedOptions.Count,
                        pageIndex,
                        pageSize
                    );

                    return new GetSurveyQuestionOptionByQuestionResult(true, "Options retrieved successfully", cachedPaginatedResult);
                }

                var questionExists = await db.SurveyQuestions
                    .AsNoTracking()
                    .AnyAsync(c => c.Id == request.QuestionId, cancellationToken);

                if (!questionExists)
                {
                    return new GetSurveyQuestionOptionByQuestionResult(false, $"Question with ID {request.QuestionId} not found");
                }

                var options = await db.SurveyQuestionOptions
                    .AsNoTracking()
                    .Where(a => a.QuestionId == request.QuestionId)
                    .OrderBy(q => q.OrderNo)
                    .ProjectTo<SurveyQuestionOptionDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                await cacheService.SetAsync(cacheKey, options, CacheConstants.DefaultExpiration);

                var pagedItems = options
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var paginatedResult = new PaginatedList<SurveyQuestionOptionDto>(
                    pagedItems,
                    options.Count,
                    pageIndex,
                    pageSize
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

