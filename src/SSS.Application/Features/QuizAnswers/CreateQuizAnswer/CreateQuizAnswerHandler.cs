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
    public class CreateQuizAnswerHandler(IAppDbContext _db, IMapper mapper)
        : IRequestHandler<CreateQuizAnswerCommand, CreateQuizAnswerResponse>
    {
        public async Task<CreateQuizAnswerResponse> Handle(CreateQuizAnswerCommand req, CancellationToken ct)
        {
            var dto = req.QuizAnswer;
            var entity = mapper.Map<QuizAnswer>(dto);
            
            entity.AnsweredAt = DateTime.UtcNow;

            await _db.QuizAnswers.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            var resultDto = mapper.Map<CreateQuizAnswerDto>(entity);
            return new CreateQuizAnswerResponse(resultDto);
        }
    }
}
