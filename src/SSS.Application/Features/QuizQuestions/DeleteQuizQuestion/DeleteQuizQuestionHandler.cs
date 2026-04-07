using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestions.DeleteQuizQuestion
{
    public class DeleteQuizQuestionHandler(IAppDbContext db)
        : IRequestHandler<DeleteQuizQuestionCommand, DeleteQuizQuestionResult>
    {
        public async Task<DeleteQuizQuestionResult> Handle(DeleteQuizQuestionCommand req, CancellationToken cancellationToken)
        {
            
            var quizQuestion = await db.QuizQuestions.FirstOrDefaultAsync(q => q.Id == req.id);
            if (quizQuestion is null)
            {
                throw new KeyNotFoundException($"Quiz question with ID '{req.id}' not found.");
            }

            db.QuizQuestions.Remove(quizQuestion);
            await db.SaveChangesAsync();
            return new DeleteQuizQuestionResult
                (
                    isDeleted: true,
                    msg: $"Quiz question with ID '{req.id}' has been deleted successfully."
                );
        }
    }
}
