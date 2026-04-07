using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyFieldSemantics.GetSurveyFieldSemanticByQuestion
{
    public class GetSurveyFieldSemanticByQuestionHandler(IAppDbContext db, IMapper mapper): IRequestHandler<GetSurveyFieldSemanticByQuestionQuery,GetSurveyFieldSemanticByQuestionResult>
    {
        public async Task<GetSurveyFieldSemanticByQuestionResult> Handle(GetSurveyFieldSemanticByQuestionQuery request, CancellationToken cancellationToken)
        {
            var entity = await db.SurveyFieldSemantics.FirstOrDefaultAsync(x => x.SurveyQuestionId == request.QuestionId, cancellationToken);
            if (entity == null)
            {
                return new GetSurveyFieldSemanticByQuestionResult(false, "Survey Field Semantic not found.");
            }
            var dto = mapper.Map<SurveyFieldSenmaticDto>(entity);
            return new GetSurveyFieldSemanticByQuestionResult(true, "Survey Field Semantic retrieved successfully.", dto);
        }
    }
}
