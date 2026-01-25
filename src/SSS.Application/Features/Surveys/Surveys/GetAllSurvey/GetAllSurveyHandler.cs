using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.GetAllSurvey
{
    public class GetAllSurveyHandler(IAppDbContext db, IMapper mapper) : IRequestHandler<GetAllSurveyQuery, GetAllSurveyResult>
    {
        public async Task<GetAllSurveyResult> Handle(GetAllSurveyQuery request, CancellationToken cancellationToken)
        {
            var query =  db.Surveys.AsNoTracking().OrderBy(x=> x.Id);

            var paginated = await PaginatedResponse<Survey>.CreateAsync(query, request.PageIndex, request.PageSize, cancellationToken);

            var result = paginated.MapItems(s => mapper.Map<SurveyDto>(s));

            return new GetAllSurveyResult(result);
            
        }
    }
}
