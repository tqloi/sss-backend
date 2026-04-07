using AutoMapper;
using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.GetSurveyTriggerMappingById
{
    public class GetSurveyTriggerMappingByIdHandler(IAppDbContext db, IMapper mapper): IRequestHandler<GetSurveyTriggerMappingByIdQuery, GetSurveyTriggerMappingByIdResult>
    {
        public async Task<GetSurveyTriggerMappingByIdResult> Handle(GetSurveyTriggerMappingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyTriggerMappings.FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new GetSurveyTriggerMappingByIdResult(false, "Survey Trigger Mapping not found.");
                }
                var dto = mapper.Map<SurveyTriggerMappingDto>(entity);
                return new GetSurveyTriggerMappingByIdResult(true, "Survey Trigger Mapping retrieved successfully.", dto);
            }
            catch (Exception ex)
            {
                return new GetSurveyTriggerMappingByIdResult(false, $"Error retrieving Survey Trigger Mapping: {ex.Message}");
            }
        }
    }
}
