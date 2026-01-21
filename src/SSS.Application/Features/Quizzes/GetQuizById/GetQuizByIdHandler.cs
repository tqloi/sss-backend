using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Quizzes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Quizzes.GetQuizById
{
    public class GetQuizByIdHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetQuizByIdRequest, GetQuizByIdResponse>
    {
        public async Task<GetQuizByIdResponse> Handle(GetQuizByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await db.Quizzes
                .AsNoTracking()
                .Include(q => q.Questions)
                .Include(s => s.StudyPlanModule)
                .Include(a => a.Attempts)
                .FirstOrDefaultAsync(q => q.Id == request.id, cancellationToken);

            if (entity == null) throw new KeyNotFoundException($"Quiz with Id {request.id} was not found.");

            var dto = mapper.Map<QuizDto>(entity);
            return new GetQuizByIdResponse(dto);
        }
    }
}
