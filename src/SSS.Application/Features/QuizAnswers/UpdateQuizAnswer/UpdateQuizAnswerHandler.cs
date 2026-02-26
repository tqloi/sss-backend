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
        : IRequestHandler<UpdateQuizAnswerCommand, UpdateQuizAnswerResult>
    {
        public async Task<UpdateQuizAnswerResult> Handle(UpdateQuizAnswerCommand request, CancellationToken cancellationToken)
        {
            var quizAnswer = await db.QuizAnswers.
                FirstOrDefaultAsync(qa => qa.Id == request.Id, cancellationToken);

            if (quizAnswer is null)
            {
                throw new Exception("Quiz answer not found.");
            }
            mapper.Map(request.UpdateQuizAnswer, quizAnswer);
            db.QuizAnswers.Update(quizAnswer);
            await db.SaveChangesAsync(cancellationToken);
            var updateQuizAnswerDto = mapper.Map<UpdateQuizAnswerDto>(quizAnswer);
            return new UpdateQuizAnswerResult(updateQuizAnswerDto);
        }
    }
}
