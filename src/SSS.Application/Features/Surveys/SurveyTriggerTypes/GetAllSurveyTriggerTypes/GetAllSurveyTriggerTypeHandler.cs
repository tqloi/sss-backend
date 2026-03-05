using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.GetAllSurveyTriggerTypes
{
    public class GetAllSurveyTriggerTypeHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetAllSurveyTriggerTypeQuery, GetAllSurveyTriggerTypeResult>
    {
        public async Task<GetAllSurveyTriggerTypeResult> Handle(
            GetAllSurveyTriggerTypeQuery request, CancellationToken cancellationToken)
        {
            var entities = await db.SurveyTriggerTypes
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Code)
                .ToListAsync(cancellationToken);

            var dtos = entities.Select(mapper.Map<SurveyTriggerTypeDto>).ToList();

            return new GetAllSurveyTriggerTypeResult(dtos);
        }
    }
}
