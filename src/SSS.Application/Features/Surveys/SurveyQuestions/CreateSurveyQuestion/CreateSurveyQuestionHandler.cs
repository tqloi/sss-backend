using AutoMapper;
using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.CreateSurveyQuestion
{
    public class CreateSurveyQuestionHandler(IAppDbContext db,IMapper mapper) : IRequestHandler<CreateSurveyQuestionCommand, CreateSurveyQuestionResponse>
    {
        public async Task<CreateSurveyQuestionResponse> Handle(CreateSurveyQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var entity = new SurveyQuestion
                //{
                //SurveyId = request.SurveyId,
                //QuestionKey = request.QuestionKey,  
                //Prompt = request.Prompt,
                //Type = request.Type,
                //OrderNo = request.OrderNo,
                //IsRequired = request.IsRequired,
                //ScaleMax = request.ScaleMax,
                //ScaleMin = request.ScaleMin,
                //};
                var entity = mapper.Map<SurveyQuestion>(request);
                await db.SurveyQuestions.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                //var dto = new SurveyQuestionDto(

                //    entity.SurveyId,
                //    entity.QuestionKey,
                //    entity.Prompt,
                //    entity.Type,
                //    entity.OrderNo,
                //    entity.IsRequired,
                //    entity.ScaleMax,
                //    entity.ScaleMin
                //    );
                var dto = mapper.Map<SurveyQuestionDto>(entity);
                return new CreateSurveyQuestionResponse(true, "Survey Created Successfully", dto);
            }
            catch (Exception ex)
            {
                return new CreateSurveyQuestionResponse(false, $"Error creating Survey: {ex.Message}", null);

            }
         
        }
    }
}
