using AutoMapper;
using SSS.Application.Common.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Quizzes.Common;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.Quizzes.GetAllQuizzesByRoadmapNodeId
{
    public class GetAllQuizzesByRoadmapNodeIdHandler(IAppDbContext _db, IMapper _mapper)
        : IRequestHandler<GetAllQuizzesByRoadmapNodeIdQuery, GetAllQuizzesByRoadmapNodeIdResult>
    {
        public async Task<GetAllQuizzesByRoadmapNodeIdResult> Handle(GetAllQuizzesByRoadmapNodeIdQuery req, CancellationToken ct)
        {    
            if (req.RoadmapNodeId == 0)
            {
                throw new ArgumentException("RoadmapNodeId is required", nameof(req.RoadmapNodeId));
            }

            var studyplanmodule = await _db.RoadmapNodes
                .AsNoTracking()
                .FirstOrDefaultAsync(spm => spm.Id == req.RoadmapNodeId, ct);

            if(studyplanmodule == null)
            {
                throw new KeyNotFoundException($"RoadmapNode with Id {req.RoadmapNodeId} not found");
            }

            var query = _db.Quizzes
                .AsNoTracking()
                .Include(q => q.Questions)
                .Include(q => q.Attempts)
                .Include(q => q.RoadmapNode)
                .Where(q => q.RoadmapNodeId == req.RoadmapNodeId)
                .OrderByDescending(q => q.CreatedAt);

            var paginatedQuizzes = await PaginatedResponse<Quiz>.CreateAsync(
                query,
                req.PageIndex,
                req.PageSize,
                ct);

            var result = paginatedQuizzes.MapItems(q => _mapper.Map<QuizDto>(q));
            return new GetAllQuizzesByRoadmapNodeIdResult(result);
        }
    }
}
