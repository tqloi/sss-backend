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

namespace SSS.Application.Features.QuizQuestionOptions.GetQuizQuestionOptionById
{
    public class GetQuizQuestionOptionByIdHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetQuizQuestionOptionByIdQuery, GetQuizQuestionOptionByIdResult>
    {
        public async Task<GetQuizQuestionOptionByIdResult> Handle(GetQuizQuestionOptionByIdQuery request, CancellationToken cancellationToken)
        {
            var quizQuestionOption = await db.QuizQuestionOptions.Where(q => q.Id == request.Id).Include(q => q.Question).FirstOrDefaultAsync();
            if (quizQuestionOption is null)
            {
                throw new KeyNotFoundException($"Quiz question option with ID '{request.Id}' not found.");
            }
            var quizQuestionOptionDto = mapper.Map<QuizQuestionOptionDto>(quizQuestionOption);
            return new GetQuizQuestionOptionByIdResult(quizQuestionOptionDto);

        }
    }
}
