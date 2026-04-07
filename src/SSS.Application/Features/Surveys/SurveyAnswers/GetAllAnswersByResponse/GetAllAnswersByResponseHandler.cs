using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.Surveys.SurveyAnswers.GetAllAnswersByResponse
{
    public class GetAllAnswersByResponseHandler(IAppDbContext db, IMapper mapper) 
        : IRequestHandler<GetAllAnswersByResponseQuery, GetAllAnswersByResponseResult>
    {
        public async Task<GetAllAnswersByResponseResult> Handle(
            GetAllAnswersByResponseQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                // Ki?m tra response có t?n t?i không
                var responseExists = await db.SurveyResponses
                    .AnyAsync(x => x.Id == request.ResponseId, cancellationToken);

                if (!responseExists)
                    throw new NotFoundException("Response not found");

                // Query answers c?a response
                var query = db.SurveyAnswers
                    .Where(x => x.ResponseId == request.ResponseId)
                    .OrderBy(x => x.QuestionId)
                    .ThenBy(x => x.AnsweredAt)
                    .AsQueryable();

                // T?o paginated response
                var paginatedAnswers = await PaginatedResponse<SurveyAnswer>.CreateAsync(
                    query, 
                    request.PageIndex, 
                    request.PageSize, 
                    cancellationToken);

                // Map sang DTO
                var paginatedDtos = paginatedAnswers.MapItems(mapper.Map<SurveyAnswerDto>);

                return new GetAllAnswersByResponseResult(
                    true, 
                    $"Found {paginatedDtos.TotalCount} answer(s)", 
                    paginatedDtos);
            }
            catch (NotFoundException ex)
            {
                return new GetAllAnswersByResponseResult(false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return new GetAllAnswersByResponseResult(
                    false, 
                    $"Error retrieving answers: {ex.Message}", 
                    null);
            }
        }
    }
}