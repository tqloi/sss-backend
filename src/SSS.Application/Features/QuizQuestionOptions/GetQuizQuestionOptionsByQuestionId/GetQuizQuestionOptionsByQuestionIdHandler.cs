using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizQuestionOptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestionOptions.GetQuizQuestionOptionsByQuestionId
{
    public class GetQuizQuestionOptionsByQuestionIdHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetQuizQuestionOptionsByQuestionIdQuery, GetQuizQuestionOptionsByQuestionIdResult>
    {
        public async Task<GetQuizQuestionOptionsByQuestionIdResult> Handle(GetQuizQuestionOptionsByQuestionIdQuery request, CancellationToken cancellationToken)
        {
            var quizQuestionOptions = await db.QuizQuestionOptions.Where(x => x.QuestionId == request.QuestionId).ToListAsync();
            if (quizQuestionOptions != null)
            {
                throw new Exception(
                    $"Quiz question options with question id {request.QuestionId} not found."
                );
            }

            var quizQuestionOptionsDto = mapper.Map<List<QuizQuestionOptionDto>>(quizQuestionOptions);
            return new GetQuizQuestionOptionsByQuestionIdResult
            (quizQuestionOptionsDto);
        }
    }
}
