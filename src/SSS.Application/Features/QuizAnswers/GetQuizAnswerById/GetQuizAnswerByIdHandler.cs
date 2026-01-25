using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAnswers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAnswers.GetQuizAnswerById
{
    public class GetQuizAnswerByIdHandler(IAppDbContext _db, IMapper mapper)
        : IRequestHandler<GetQuizAnswerByIdQuery, GetQuizAnswerByIdResponse>
    {
        public async Task<GetQuizAnswerByIdResponse> Handle(GetQuizAnswerByIdQuery req, CancellationToken ct)
        {
            var entity = await _db.QuizAnswers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == req.id, ct);
            if (entity == null)
            {
                throw new KeyNotFoundException($"QuizAnswer with Id {req.id} not found.");
            }
            var dto = mapper.Map<QuizAnswerDto>(entity);
            return new GetQuizAnswerByIdResponse(dto);
        }
    }
}
