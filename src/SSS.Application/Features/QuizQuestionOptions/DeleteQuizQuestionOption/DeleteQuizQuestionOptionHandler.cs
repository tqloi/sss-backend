using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestionOptions.DeleteQuizQuestionOption
{
    public class DeleteQuizQuestionOptionHandler(IAppDbContext db)
        : IRequestHandler<DeleteQuizQuestionOptionCommand, DeleteQuizQuestionOptionResult>
    {
        public async Task<DeleteQuizQuestionOptionResult> Handle(DeleteQuizQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            
            var entity = await db.QuizQuestionOptions.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                throw new KeyNotFoundException("Quiz question option not found.");
            }
            db.QuizQuestionOptions.Remove(entity);
            await db.SaveChangesAsync();
            return new DeleteQuizQuestionOptionResult(msg: "Completed", isDeleted: true);
            
        }
    }
}
