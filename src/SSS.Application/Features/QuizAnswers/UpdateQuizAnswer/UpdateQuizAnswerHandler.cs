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

namespace SSS.Application.Features.QuizAnswers.UpdateQuizAnswer
{
    public class UpdateQuizAnswerHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<UpdateQuizAnswerCommand, UpdateQuizAnswerResponse>
    {
        public async Task<UpdateQuizAnswerResponse> Handle(UpdateQuizAnswerCommand req, CancellationToken ct)
        {
            var entity = await db.QuizAnswers
                .FirstOrDefaultAsync(q => q.Id == req.Id, ct);

            if (entity == null) throw new KeyNotFoundException($"QuizAnswer with Id {req.Id} was not found.");

            mapper.Map(req.UpdateQuizAnswerDto, entity);

            db.QuizAnswers.Update(entity);
            await db.SaveChangesAsync(ct);

            var dto = mapper.Map<UpdateQuizAnswerDto>(entity);
            return new UpdateQuizAnswerResponse(dto);
        }
    }
}
