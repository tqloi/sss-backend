using AutoMapper;
using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Quizzes.Common;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Quizzes.CreateQuiz
{
    public class CreateQuizHandler(IAppDbContext _db, IMapper _mapper)
        : IRequestHandler<CreateQuizCommand, CreateQuizResult>
    {
        public async Task<CreateQuizResult> Handle(CreateQuizCommand req, CancellationToken ct)
        {
            var dto = req.CreateQuizNode;
            var entity = _mapper.Map<Quiz>(dto);

            entity.CreatedAt = DateTime.UtcNow;

            await _db.Quizzes.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            var createdDto = _mapper.Map<CreateQuizDto>(entity);

            return new CreateQuizResult(createdDto);
        }
    }
}
