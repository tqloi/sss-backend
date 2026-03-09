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

namespace SSS.Application.Features.QuizAnswers.GetQuizAnswerByAttemptId
{
    public class GetQuizAnswerByAttemptIdHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetQuizAnswerByAttemptIdQuery, GetQuizAnswerByAttemptIdResult>
    {
        public async Task<GetQuizAnswerByAttemptIdResult> Handle(GetQuizAnswerByAttemptIdQuery req, CancellationToken ct)
        {
            var quizAnswer = await db.QuizAnswers.FirstOrDefaultAsync(x => x.AttemptId == req.attemptId && x.QuestionId == req.questionId);
            if (quizAnswer is null)
               throw new Exception($"Quiz answer with attempt id {req.attemptId} and question id {req.questionId} not found.");
            var result = mapper.Map<QuizAnswerDto>(quizAnswer);
            return new GetQuizAnswerByAttemptIdResult(result);

        }
    }
}
