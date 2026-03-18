using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizQuestions.Common;

namespace SSS.Application.Features.QuizQuestions.GetAllQuizQuestionsByQuizId
{
    public class GetAllQuizQuestionsByQuizIdHandler(IAppDbContext db, IMapper mapper)
            : IRequestHandler<GetAllQuizQuestionsByQuizIdQuery, GetAllQuizQuestionsByQuizIdResult>
    {
        public async Task<GetAllQuizQuestionsByQuizIdResult> Handle(GetAllQuizQuestionsByQuizIdQuery req, CancellationToken cancellationToken)
        {
            var quizQuestions = await db.QuizQuestions
                .Where(q => q.QuizId == req.quizId)
                .Include(q => q.Options)
                .OrderBy(q => q.OrderNo)
                .ToListAsync(cancellationToken);

            var quizQuestionDtos = mapper.Map<List<QuizQuestionDto>>(quizQuestions);
            return new GetAllQuizQuestionsByQuizIdResult(quizQuestionDtos);
        }
    }
}
