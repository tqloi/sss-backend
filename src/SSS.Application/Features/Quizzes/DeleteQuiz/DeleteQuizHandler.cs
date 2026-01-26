using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Quizzes.DeleteQuiz
{
    public class DeleteQuizHandler(IAppDbContext _db)
        : IRequestHandler<DeleteQuizRequest, DeleteQuizResponse>
    {
        public async Task<DeleteQuizResponse> Handle(DeleteQuizRequest req, CancellationToken ct)
        {
            var entity = await _db.Quizzes
                .FirstOrDefaultAsync(q => q.Id == req.QuizId, ct);

            if(entity == null)
            {
                throw new KeyNotFoundException($"Quiz with ID '{req.QuizId}' not found.");
            }

            _db.Quizzes.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return new DeleteQuizResponse(                
                IsDeleted: true,
                msg: "Quiz deleted successfully."
            );
        }
    }
}
