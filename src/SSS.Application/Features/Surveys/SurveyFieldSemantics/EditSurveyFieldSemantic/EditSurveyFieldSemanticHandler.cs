using AutoMapper;
using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.EditSurveyQuestionOption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyFieldSemantics.EditSurveyFieldSemantic
{
    public class EditSurveyFieldSemanticHandler(IAppDbContext db) : IRequestHandler<EditSurveyFieldSemanticCommand, EditSurveyFieldSemanticResponse>
    {
        public async Task<EditSurveyFieldSemanticResponse> Handle(EditSurveyFieldSemanticCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyFieldSemantics.FindAsync(new object?[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new EditSurveyFieldSemanticResponse(false, "SurveyFieldSemantic not found.");
                }

                entity.SurveyQuestionId = request.SurveyQuestionId;
                entity.DimensionCode = request.DimensionCode;
                entity.Evaluates = request.Evaluates;
                entity.AIHint = request.AIHint;
                entity.Weight = request.Weight;
                entity.CreatedAt = request.CreatedAt;

                await db.SaveChangesAsync(cancellationToken);
                return new EditSurveyFieldSemanticResponse(true, "SurveyFieldSemantic updated successfully.");
            }
            catch (Exception ex)
            {
                return new EditSurveyFieldSemanticResponse(false, $"Error editing SurveyFieldSemantic: {ex.Message}");
            }
        }
    }
}
