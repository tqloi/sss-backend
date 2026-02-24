using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAnswers.DeleteQuizAnswers
{
    public class DeleteQuizAnswersHandler(IAppDbContext db)
        : IRequestHandler<DeleteQuizAnswersCommand, DeleteQuizAnswersResult>
    {
        public async Task<DeleteQuizAnswersResult> Handle(DeleteQuizAnswersCommand req, CancellationToken cancellationToken)
        {
            var quizAnswer = db.QuizAnswers.Where(q => q.Id == req.id).FirstOrDefault();
            if (quizAnswer is null)
            {
                throw new KeyNotFoundException($"Quiz answer with ID '{req.id}' not found.");
            }

            db.QuizAnswers.Remove(quizAnswer);
           await db.SaveChangesAsync();
            return new DeleteQuizAnswersResult
            (
                IsDeleted: true,
                msg : $"Quiz answer with ID '{req.id}' has been deleted successfully."
            );

        }
    }
}
