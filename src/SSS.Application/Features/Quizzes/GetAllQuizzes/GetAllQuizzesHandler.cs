using AutoMapper;
using SSS.Application.Common.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Quizzes.Common;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.Quizzes.GetAllQuizzes
{
    public class GetAllQuizzesHandler(IAppDbContext _db, IMapper mapper)
        : IRequestHandler<GetAllQuizzesQuery, GetAllQuizzesResult>
    {
        public async Task<GetAllQuizzesResult> Handle(GetAllQuizzesQuery req, CancellationToken ct)
        {
            var query = _db.Quizzes
                .AsNoTracking()
                .OrderByDescending(q => q.CreatedAt);

            var paginated = await PaginatedResponse<Quiz>
                .CreateAsync(query, req.PageIndex, req.PageSize, ct);

            var result = paginated
                .MapItems(q => mapper.Map<QuizDto>(q));
            return new GetAllQuizzesResult(result);
        }
    }
}
