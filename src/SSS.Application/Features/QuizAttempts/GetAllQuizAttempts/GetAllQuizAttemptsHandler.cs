using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.QuizAttempts.Common;
using SSS.Domain.Entities.Assessment;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SSS.Application.Features.QuizAttempts.GetAllQuizAttempts
{
    public class GetAllQuizAttemptsHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetAllQuizAttemptsQuery, GetAllQuizAttemptsResult>
    {
        public async Task<GetAllQuizAttemptsResult> Handle(GetAllQuizAttemptsQuery req, CancellationToken ct)
        {

            var query =  db.QuizAttempts
                .AsNoTracking()
                .Include(qa => qa.Quiz)
                .Include(qa => qa.User)
                .OrderByDescending(a => a.SubmittedAt);


            var paginated = await PaginatedResponse<QuizAttempt>
                .CreateAsync(query, req.PageIndex, req.PageSize, ct);

            var result = paginated
                .MapItems(qa => mapper.Map<QuizAttemptDto>(qa));

            return new GetAllQuizAttemptsResult(result);

        }
    }
}
