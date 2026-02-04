using AutoMapper;
using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.CreateSurveyTriggerMapping
{
    public class CreateSurveyTriggerMappingHandler(IAppDbContext db, IMapper mapper): IRequestHandler<CreateSurveyTriggerMappingCommand, CreateSurveyTriggerMappingResponse>
    {
        public async Task<CreateSurveyTriggerMappingResponse> Handle(CreateSurveyTriggerMappingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = mapper.Map<SurveyTriggerMapping>(request);
                await db.SurveyTriggerMappings.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);
                var dto = mapper.Map<SurveyTriggerMappingDto>(entity);
                return new CreateSurveyTriggerMappingResponse(true, "Survey Trigger Mapping Created Successfully", dto);
            }
            catch (Exception ex)
            {
                return new CreateSurveyTriggerMappingResponse(false, $"Error while creating Survey Trigger Mapping: {ex.Message}");
            }
        }
    }
}
