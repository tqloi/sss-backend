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

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption
{
    public class CreateSurveyQuestionOptionHandler(IAppDbContext db,IMapper mapper) : IRequestHandler<CreateSurveyQuestionOptionCommand, CreateSurveyQuestionOptionResponse>
    {
        public async Task<CreateSurveyQuestionOptionResponse> Handle(CreateSurveyQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = mapper.Map<SurveyQuestionOption>(request);
                await db.SurveyQuestionOptions.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                var dto = mapper.Map<SurveyQuestionOptionDto>(entity);
                return new CreateSurveyQuestionOptionResponse(true, "SurveyQuestion Option Created Successfully", dto);
            }
            catch (Exception ex)
            {
                return new CreateSurveyQuestionOptionResponse(false, $"Error while creating option: {ex.Message}");
            }
        }
    }
}
