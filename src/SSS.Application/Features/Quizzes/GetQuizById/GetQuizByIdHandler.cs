using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Quizzes.Common;

namespace SSS.Application.Features.Quizzes.GetQuizById
{
    public class GetQuizByIdHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetQuizByIdQuery, GetQuizByIdResult>
    {
        public async Task<GetQuizByIdResult> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await db.Quizzes
                .AsNoTracking()
                .Include(q => q.Questions)
                .Include(s => s.RoadmapNode)
                .Include(a => a.Attempts)
                .FirstOrDefaultAsync(q => q.Id == request.id, cancellationToken);

            if (entity == null) throw new KeyNotFoundException($"Quiz with Id {request.id} was not found.");

            var dto = mapper.Map<QuizDto>(entity);
            return new GetQuizByIdResult(dto);
        }
    }
}
