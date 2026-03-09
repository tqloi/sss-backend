using AutoMapper;
using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.CreateSurveyTriggerType
{
    public class CreateSurveyTriggerTypeHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<CreateSurveyTriggerTypeCommand, CreateSurveyTriggerTypeResponse>
    {
        public async Task<CreateSurveyTriggerTypeResponse> Handle(
            CreateSurveyTriggerTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = mapper.Map<SurveyTriggerType>(request);
                await db.SurveyTriggerTypes.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                var dto = mapper.Map<SurveyTriggerTypeDto>(entity);
                return new CreateSurveyTriggerTypeResponse(true, "SurveyTriggerType created successfully.", dto);
            }
            catch (Exception ex)
            {
                return new CreateSurveyTriggerTypeResponse(false, $"Error creating SurveyTriggerType: {ex.Message}");
            }
        }
    }
}
