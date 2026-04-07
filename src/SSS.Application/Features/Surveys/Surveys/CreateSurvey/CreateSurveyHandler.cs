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

namespace SSS.Application.Features.Surveys.Surveys.CreateSurvey
{
    public class CreateSurveyHandler(IAppDbContext db, IMapper mapper) : IRequestHandler<CreateSurveyCommand, CreateSurveyResponse>
    {
        public async Task<CreateSurveyResponse> Handle(CreateSurveyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var entity = new Survey
                //{

                //    Code = request.Code,
                //    Title = request.Title,
                //    Status = request.Status
                //};
                var entity = mapper.Map<Survey>(request);

                await db.Surveys.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                //var dto = new SurveyDto(

                //    entity.Title,
                //    entity.Code,
                //    entity.Status);
                var dto = mapper.Map<SurveyDto>(entity);
                return new CreateSurveyResponse(true, "Survey Created Successfully", dto);
            }
            catch (Exception ex)
            {
                return new CreateSurveyResponse(false, $"Error creating Survey: {ex.Message}", null);

            }
        }
    }
}
