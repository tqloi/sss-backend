using AutoMapper;
using LecX.Application.Common.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Quizzes.Common;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.Quizzes.GetAllQuizzesByStudyPlanModuleId
{
    public class GetAllQuizzesByStudyPlanModuleIdHandler(IAppDbContext _db, IMapper _mapper)
        : IRequestHandler<GetAllQuizzesByStudyPlanModuleIdRequest, GetAllQuizzesByStudyPlanModuleIdResponse>
    {
        public async Task<GetAllQuizzesByStudyPlanModuleIdResponse> Handle(GetAllQuizzesByStudyPlanModuleIdRequest req, CancellationToken ct)
        {    
            if (req.StudyPlanModuleId == 0)
            {
                throw new ArgumentException("StudyPlanModuleId is required", nameof(req.StudyPlanModuleId));
            }

            var studyplanmodule = await _db.StudyPlanModules
                .AsNoTracking()
                .FirstOrDefaultAsync(spm => spm.Id == req.StudyPlanModuleId, ct);

            if(studyplanmodule == null)
            {
                throw new KeyNotFoundException($"StudyPlanModule with Id {req.StudyPlanModuleId} not found");
            }

            var query = _db.Quizzes
                .AsNoTracking()
                .Include(q => q.Questions)
                .Include(q => q.Attempts)
                .Include(q => q.StudyPlanModule)
                .Where(q => q.StudyPlanModuleId == req.StudyPlanModuleId)
                .OrderByDescending(q => q.CreatedAt);

            var paginatedQuizzes = await PaginatedResponse<Quiz>.CreateAsync(
                query,
                req.PageIndex,
                req.PageSize,
                ct);

            var result = paginatedQuizzes.MapItems(q => _mapper.Map<QuizDto>(q));
            return new GetAllQuizzesByStudyPlanModuleIdResponse(result);

        }
    }
}
