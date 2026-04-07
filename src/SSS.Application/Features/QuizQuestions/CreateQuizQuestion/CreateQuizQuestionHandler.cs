using AutoMapper;
using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizQuestions.Common;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestions.CreateQuizQuestion
{
    public class CreateQuizQuestionHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<CreateQuizQuestionCommand, CreateQuizQuestionResult>
    {
        public async Task<CreateQuizQuestionResult> Handle(CreateQuizQuestionCommand req, CancellationToken cancellationToken)
        {
            
            var dto = req.CreateQuizQuestionDto;

            var entity = mapper.Map<QuizQuestion>(dto);

            await db.QuizQuestions.AddAsync(entity, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            var resultDto = mapper.Map<CreateQuizQuestionDto>(entity);
            return new CreateQuizQuestionResult(resultDto);
        }
    }
}
