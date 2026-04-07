using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.CreateSurveyResponse
{
    public  class CreateSurveyResponseHandler(IAppDbContext db, IMapper mapper): IRequestHandler<CreateSurveyResponseCommand, CreateSurveyResponseResponse>
    {
        public async Task<CreateSurveyResponseResponse> Handle(CreateSurveyResponseCommand request, CancellationToken cancellationToken)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(request.UserId))
                    throw new ForbiddenException("Unauthorized");

                var surveyExists = await db.Surveys
               .AnyAsync(x => x.Id == request.SurveyId, cancellationToken);
                if (!surveyExists)
                    throw new NotFoundException("Survey not found");


                var existingResponse = await db.SurveyResponses
                .FirstOrDefaultAsync(x =>
                x.SurveyId == request.SurveyId &&
                x.UserId == request.UserId,
                cancellationToken);

                if (existingResponse != null)
                {
                    //Nếu đã submit rồi thì tuỳ rule
                    if (existingResponse.SubmittedAt != null)
                        throw new ForbiddenException("Survey already submitted");

                    //Nếu đang làm dở → trả về response cũ
                    var existingDto = mapper.Map<SurveyResponseDto>(existingResponse);
                    return new CreateSurveyResponseResponse(
                        true,
                        "Survey response already started",
                        existingDto
                    );
                }

                var entity = mapper.Map<SurveyResponse>(request);

                await db.SurveyResponses.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                var dto = mapper.Map<SurveyResponseDto>(entity);
                return new CreateSurveyResponseResponse(true, "Survey response created successfully",dto);
            }
            catch (Exception ex)
            {
                return new CreateSurveyResponseResponse(false, $"Error creating survey response: {ex.Message}");
            }
        }
    }
    
    }

