using AutoMapper;
using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizQuestionOptions.Common;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestionOptions.CreateQuizQuestionOption
{
    public class CreateQuizQuestionOptionHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<CreateQuizQuestionOptionCommand, CreateQuizQuestionOptionResult>
    {
        public async Task<CreateQuizQuestionOptionResult> Handle(CreateQuizQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            var quizQuestionOption = mapper.Map<QuizQuestionOption>(request.CreateQuizQuestionOptionDto);
            db.QuizQuestionOptions.Add(quizQuestionOption);

            await db.SaveChangesAsync();
            var createQuizQuestionOptionDto = mapper.Map<CreateQuizQuestionOptionDto>(quizQuestionOption);
            return new CreateQuizQuestionOptionResult(createQuizQuestionOptionDto);

        }
    }
}
