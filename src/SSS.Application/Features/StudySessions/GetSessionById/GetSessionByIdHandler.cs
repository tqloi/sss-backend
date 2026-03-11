using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.GetSessionById
{
    public class GetSessionByIdHandler(IAppDbContext context, IMapper mapper)
        : IRequestHandler<GetSessionByIdQuery, GetSessionByIdResult>
    {
        public async Task<GetSessionByIdResult> Handle(GetSessionByIdQuery req, CancellationToken ct)
        {
            var dto = await context.StudySessions
                .AsNoTracking()
                .Include(s => s.Node)
                .Include(s => s.StudyPlan)
                    .ThenInclude(sp => sp!.Roadmap)
                .Where(s => s.Id == req.SessionId && s.UserId == req.UserId)
                .ProjectTo<SessionDetailDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            if (dto == null)
                throw new NotFoundException($"Session {req.SessionId} not found");

            return new GetSessionByIdResult
            {
                Success = true,
                Data = dto
            };
        }
    }
}
