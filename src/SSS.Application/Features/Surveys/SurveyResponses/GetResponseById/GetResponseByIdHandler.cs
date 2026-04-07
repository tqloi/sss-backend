using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.GetResponseById
{
    public class GetResponseByIdHandler(IAppDbContext db, IMapper mapper) : IRequestHandler<GetResponseByIdQuery, GetResponseByIdResult>
    {
        public async Task<GetResponseByIdResult> Handle(GetResponseByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await db.SurveyResponses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (response == null)
                {
                    return new GetResponseByIdResult(false, "Response Not Found");
                }

                var dto = mapper.Map<SurveyResponseDto>(response);

                return new GetResponseByIdResult(true, "Success", dto);
            }
            catch (Exception ex)
            {
                return new GetResponseByIdResult(false, $"Error retrieving response: {ex.Message}");

            }
        }
    }
}
