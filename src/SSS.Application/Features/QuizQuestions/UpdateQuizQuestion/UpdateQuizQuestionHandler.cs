using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizQuestions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestions.UpdateQuizQuestion
{
    public class UpdateQuizQuestionHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<UpdateQuizQuestionCommand, UpdateQuizQuestionResult>
    {
        public async Task<UpdateQuizQuestionResult> Handle(UpdateQuizQuestionCommand request, CancellationToken cancellationToken)
        {

            var quizQuestion = await db.QuizQuestions.FirstOrDefaultAsync(q => q.Id == request.Id);

            if (quizQuestion is null)
            {
                throw new Exception($"QuizQuestion with Id {request.Id} not found.");
            }

            mapper.Map(request.UpdateQuizQuestionDto, quizQuestion);

            db.QuizQuestions.Update(quizQuestion);
            await db.SaveChangesAsync(cancellationToken);

            var updateQuizQuestionDto = mapper.Map<UpdateQuizQuestionDto>(quizQuestion);
            return new UpdateQuizQuestionResult(updateQuizQuestionDto);

        }
    }
}
