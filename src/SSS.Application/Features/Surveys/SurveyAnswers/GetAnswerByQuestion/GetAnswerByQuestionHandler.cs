using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyAnswers.GetAnswerByQuestion
{
    public class GetAnswerByQuestionHandler(IAppDbContext db, IMapper mapper) 
        : IRequestHandler<GetAnswerByQuestionQuery, GetAnswerByQuestionResult>
    {
        public async Task<GetAnswerByQuestionResult> Handle(
            GetAnswerByQuestionQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                // Ki?m tra response có t?n t?i không
                var responseExists = await db.SurveyResponses
                    .AnyAsync(x => x.Id == request.ResponseId, cancellationToken);

                if (!responseExists)
                    throw new NotFoundException("Response not found");

                // Ki?m tra question có t?n t?i không
                var questionExists = await db.SurveyQuestions
                    .AnyAsync(x => x.Id == request.QuestionId, cancellationToken);

                if (!questionExists)
                    throw new NotFoundException("Question not found");

                // L?y answer
                var answer = await db.SurveyAnswers
                    .FirstOrDefaultAsync(x => x.ResponseId == request.ResponseId 
                        && x.QuestionId == request.QuestionId, cancellationToken);

                if (answer == null)
                    throw new NotFoundException("Answer not found for this question in the response");

                var dto = mapper.Map<SurveyAnswerDto>(answer);

                return new GetAnswerByQuestionResult(
                    true, 
                    "Answer retrieved successfully", 
                    dto);
            }
            catch (NotFoundException ex)
            {
                return new GetAnswerByQuestionResult(false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return new GetAnswerByQuestionResult(
                    false, 
                    $"Error retrieving answer: {ex.Message}", 
                    null);
            }
        }
    }
}