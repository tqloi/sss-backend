using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizQuestionOptions.Common;

namespace SSS.Application.Features.QuizQuestionOptions.UpdateQuizQuestionOption
{
    public class UpdateQuizQuestionOptionHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<UpdateQuizQuestionOptionCommand, UpdateQuizQuestionOptionResult>
    {
        public async Task<UpdateQuizQuestionOptionResult> Handle(UpdateQuizQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            var quizQuestionOption = await db.QuizQuestionOptions
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (quizQuestionOption is null)
            {
                throw new KeyNotFoundException($"QuizQuestionOption with id {request.Id} not found.");
            }

            mapper.Map(request.UpdateQuizQuestionOptionDto, quizQuestionOption);
            db.QuizQuestionOptions.Update(quizQuestionOption);
            await db.SaveChangesAsync(cancellationToken);

            var updatedQuizQuestionOptionDto = mapper.Map<UpdateQuizQuestionOptionDto>(quizQuestionOption);
            return new UpdateQuizQuestionOptionResult(updatedQuizQuestionOptionDto);
        }
    }
}
