using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyAnswers.UpdateAnswer
{
    public class UpdateAnswerHandler(IAppDbContext db, IMapper mapper) 
        : IRequestHandler<UpdateAnswerCommand, UpdateAnswerResponse>
    {
        public async Task<UpdateAnswerResponse> Handle(
            UpdateAnswerCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                var answer = await db.SurveyAnswers
                    .Include(a => a.Response)
                    .Include(a => a.Question)
                    .FirstOrDefaultAsync(x => x.Id == request.AnswerId, cancellationToken);

                if (answer == null)
                    throw new NotFoundException("Answer not found");

                // Ki?m tra response ?ã submit ch?a
                if (answer.Response.SubmittedAt != null)
                    throw new ForbiddenException("Cannot update answer of submitted response");

                // Ki?m tra n?u có OptionId thì Option ph?i t?n t?i và thu?c v? Question này
                if (request.OptionId.HasValue)
                {
                    var optionExists = await db.SurveyQuestionOptions
                        .AnyAsync(x => x.Id == request.OptionId.Value 
                            && x.QuestionId == answer.QuestionId, cancellationToken);
                    
                    if (!optionExists)
                        throw new NotFoundException("Option not found for this question");
                }

                // Update answer
                answer.OptionId = request.OptionId;
                answer.NumberValue = request.NumberValue;
                answer.TextValue = request.TextValue;
                answer.AnsweredAt = DateTime.UtcNow;

                await db.SaveChangesAsync(cancellationToken);

                var dto = mapper.Map<SurveyAnswerDto>(answer);
                return new UpdateAnswerResponse(true, "Answer updated successfully", dto);
            }
            catch (NotFoundException ex)
            {
                return new UpdateAnswerResponse(false, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                return new UpdateAnswerResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new UpdateAnswerResponse(false, $"Error updating answer: {ex.Message}");
            }
        }
    }
}