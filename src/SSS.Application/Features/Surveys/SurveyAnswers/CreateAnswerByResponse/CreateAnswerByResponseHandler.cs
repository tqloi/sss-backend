using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.Surveys.SurveyAnswers.CreateAnswerByResponse
{
    public class CreateAnswerByResponseHandler(IAppDbContext db, IMapper mapper) 
        : IRequestHandler<CreateAnswerByResponseCommand, CreateAnswerByResponseResponse>
    {
        public async Task<CreateAnswerByResponseResponse> Handle(
            CreateAnswerByResponseCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                // Ki?m tra Response có t?n t?i không
                var responseExists = await db.SurveyResponses
                    .AnyAsync(x => x.Id == request.ResponseId, cancellationToken);
                
                if (!responseExists)
                    throw new NotFoundException("Response not found");

                // Ki?m tra Response ?ã submit ch?a
                var response = await db.SurveyResponses
                    .FirstOrDefaultAsync(x => x.Id == request.ResponseId, cancellationToken);
                
                if (response?.SubmittedAt != null)
                    throw new ForbiddenException("Cannot add answer to submitted response");

                // Ki?m tra Question có t?n t?i không
                var questionExists = await db.SurveyQuestions
                    .AnyAsync(x => x.Id == request.QuestionId, cancellationToken);
                
                if (!questionExists)
                    throw new NotFoundException("Question not found");

                // Ki?m tra n?u có OptionId thì Option ph?i t?n t?i
                if (request.OptionId.HasValue)
                {
                    var optionExists = await db.SurveyQuestionOptions
                        .AnyAsync(x => x.Id == request.OptionId.Value 
                            && x.QuestionId == request.QuestionId, cancellationToken);
                    
                    if (!optionExists)
                        throw new NotFoundException("Option not found for this question");
                }

                // Ki?m tra xem ?ã có answer cho question này ch?a
                var existingAnswer = await db.SurveyAnswers
                    .FirstOrDefaultAsync(x => x.ResponseId == request.ResponseId 
                        && x.QuestionId == request.QuestionId, cancellationToken);

                if (existingAnswer != null)
                {
                    // Update existing answer
                    existingAnswer.OptionId = request.OptionId;
                    existingAnswer.NumberValue = request.NumberValue;
                    existingAnswer.TextValue = request.TextValue;
                    existingAnswer.AnsweredAt = request.AnsweredAt;

                    await db.SaveChangesAsync(cancellationToken);

                    var updatedDto = mapper.Map<SurveyAnswerDto>(existingAnswer);
                    return new CreateAnswerByResponseResponse(
                        true, 
                        "Answer updated successfully", 
                        updatedDto);
                }

                // T?o answer m?i
                var entity = mapper.Map<SurveyAnswer>(request);
                
                await db.SurveyAnswers.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                var dto = mapper.Map<SurveyAnswerDto>(entity);
                return new CreateAnswerByResponseResponse(
                    true, 
                    "Answer created successfully", 
                    dto);
            }
            catch (NotFoundException ex)
            {
                return new CreateAnswerByResponseResponse(false, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                return new CreateAnswerByResponseResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new CreateAnswerByResponseResponse(
                    false, 
                    $"Error creating answer: {ex.Message}");
            }
        }
    }
}