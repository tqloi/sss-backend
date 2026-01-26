using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Quizzes.Common;
using SSS.Application.Features.Quizzes.UpdateQuizNode;

namespace SSS.Application.Features.Quizzes.UpdateQuiz
{
    public class UpdateQuizNodeHandler(IAppDbContext _db, IMapper _mapper)
        : IRequestHandler<UpdateQuizCommand, UpdateQuizResult>
    {
        public async Task<UpdateQuizResult> Handle(UpdateQuizCommand req, CancellationToken ct)
        {
            var entity = await _db.Quizzes
                .FirstOrDefaultAsync(c => c.Id == req.Id, ct);

            if (entity == null) throw new KeyNotFoundException($"Quiz with Id {req.Id} was not found.");

            _mapper.Map(req.UpdateQuizNodeDto, entity);

             _db.Quizzes.Update(entity);
            await _db.SaveChangesAsync(ct);

            var res = _mapper.Map<UpdateQuizDto>(entity);
            return new UpdateQuizResult(res);
        }
    }
}
