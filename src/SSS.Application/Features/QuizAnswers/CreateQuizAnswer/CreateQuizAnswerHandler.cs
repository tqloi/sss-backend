using AutoMapper;
using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAnswers.Common;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAnswers.CreateQuizAnswer
{
    public class CreateQuizAnswerHandler(IAppDbContext _db, IMapper _mapper)
        : IRequestHandler<CreateQuizAnswerCommand, CreateQuizAnswerResult>
    {
        public async Task<CreateQuizAnswerResult> Handle(CreateQuizAnswerCommand req, CancellationToken ct)
        {
            var dto = req.CreateQuizAnswer;

            var entity = _mapper.Map<QuizAnswer>(dto);

            entity.AttemptId = dto.AttemptId;
            entity.AnsweredAt = DateTime.UtcNow;

            await _db.QuizAnswers.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            var createdDto = _mapper.Map<CreateQuizAnswerDto>(entity);

            return new CreateQuizAnswerResult(createdDto);
        }
    }
}
