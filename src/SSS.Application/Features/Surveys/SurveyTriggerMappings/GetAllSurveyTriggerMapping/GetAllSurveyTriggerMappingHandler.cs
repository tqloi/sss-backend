using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using SSS.Application.Features.Surveys.Surveys.GetAllSurvey;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.GetAllSurveyTriggerMapping
{
    public class GetAllSurveyTriggerMappingHandler(IAppDbContext db, IMapper mapper): IRequestHandler<GetAllSurveyTriggerMappingQuery, GetAllSurveyTriggerMappingResult>
    {
        public async Task<GetAllSurveyTriggerMappingResult> Handle(GetAllSurveyTriggerMappingQuery request, CancellationToken cancellationToken)
        {
            var query = db.SurveyTriggerMappings.AsNoTracking().OrderBy(x => x.Id);

            var paginated = await PaginatedResponse<SurveyTriggerMapping>.CreateAsync(query, request.PageIndex, request.PageSize, cancellationToken);

            var result = paginated.MapItems(s => mapper.Map<SurveyTriggerMappingDto>(s));

            return new GetAllSurveyTriggerMappingResult(result);

        }
    }
}
