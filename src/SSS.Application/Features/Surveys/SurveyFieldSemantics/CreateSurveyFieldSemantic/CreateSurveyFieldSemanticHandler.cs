using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption;
using SSS.Application.Features.Surveys.SurveyResponses.CreateSurveyResponse;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyFieldSemantics.CreateSurveyFieldSemantic
{
    public class CreateSurveyFieldSemanticHandler(IAppDbContext db, IMapper mapper):
        IRequestHandler<CreateSurveyFieldSemanticCommand, CreateSurveyFieldSemanticResponse>

    {
        public async Task<CreateSurveyFieldSemanticResponse> Handle(CreateSurveyFieldSemanticCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await db.SurveyFieldSemantics
                    .AnyAsync(x => x.SurveyQuestionId == request.SurveyQuestionId, cancellationToken);

                if (exists)
                {
                    return new CreateSurveyFieldSemanticResponse(false, $"SurveyFieldSemantic with QuestionId {request.SurveyQuestionId} already exists");
                }

                var entity = mapper.Map<SurveyFieldSemantic>(request);
                await db.SurveyFieldSemantics.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                var dto = mapper.Map<SurveyFieldSenmaticDto>(entity);
                return new CreateSurveyFieldSemanticResponse(true, "SurveyFieldSemantic Created Successfully", dto);
            }
            catch (Exception ex)
            {
                return new CreateSurveyFieldSemanticResponse(false, $"Error while creating SurveyFieldSemantic: {ex.Message}");
            }
        }
    }
}
