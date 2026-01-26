using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Domain.Entities.Assessment;
using System.Text.Json;

namespace SSS.Application.Features.Surveys.Surveys.TakeSurvey
{
    public class TakeSurveyHandler(IAppDbContext db) 
        : IRequestHandler<TakeSurveyCommand, TakeSurveyResponse>
    {
        public async Task<TakeSurveyResponse> Handle(
            TakeSurveyCommand request, 
            CancellationToken cancellationToken)
        {
            await db.BeginTransactionAsync(cancellationToken);
            
            try
            {
                // 1. Validate survey exists
                var survey = await db.Surveys
                    .Include(s => s.Questions.OrderBy(q => q.OrderNo))
                    .FirstOrDefaultAsync(x => x.Id == request.SurveyId, cancellationToken);

                if (survey == null)
                    throw new NotFoundException("Survey not found");

                if (string.IsNullOrWhiteSpace(request.UserId))
                    throw new ForbiddenException("Unauthorized");

                // 2. Find or create response
                var response = await db.SurveyResponses
                    .FirstOrDefaultAsync(x => 
                        x.SurveyId == request.SurveyId && 
                        x.UserId == request.UserId &&
                        x.SubmittedAt == null, // Only get InProgress response
                        cancellationToken);

                if (response == null)
                {
                    response = new SurveyResponse
                    {
                        SurveyId = request.SurveyId,
                        UserId = request.UserId,
                        StartedAt = request.StartedAt,
                        TriggerReason = request.TriggerReason
                    };
                    await db.SurveyResponses.AddAsync(response, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    // Update existing response
                    response.StartedAt = request.StartedAt;
                    response.TriggerReason = request.TriggerReason;
                }

                // 3. Process answers
                var validationErrors = new List<string>();
                var processedCount = 0;

                foreach (var answerInput in request.Answers)
                {
                    // Validate question exists in survey
                    var question = survey.Questions.FirstOrDefault(q => q.Id == answerInput.QuestionId);
                    if (question == null)
                    {
                        validationErrors.Add($"Question {answerInput.QuestionId} not found in survey");
                        continue;
                    }

                    // Validate option if provided
                    if (answerInput.OptionId.HasValue)
                    {
                        var optionExists = await db.SurveyQuestionOptions
                            .AnyAsync(o => 
                                o.Id == answerInput.OptionId.Value && 
                                o.QuestionId == answerInput.QuestionId, 
                                cancellationToken);

                        if (!optionExists)
                        {
                            validationErrors.Add($"Option {answerInput.OptionId} not found for question {answerInput.QuestionId}");
                            continue;
                        }
                    }

                    // Find or create answer
                    var existingAnswer = await db.SurveyAnswers
                        .FirstOrDefaultAsync(a => 
                            a.ResponseId == response.Id && 
                            a.QuestionId == answerInput.QuestionId, 
                            cancellationToken);

                    if (existingAnswer != null)
                    {
                        // Update existing answer
                        existingAnswer.OptionId = answerInput.OptionId;
                        existingAnswer.NumberValue = answerInput.NumberValue;
                        existingAnswer.TextValue = answerInput.TextValue;
                        existingAnswer.AnsweredAt = answerInput.AnsweredAt;
                    }
                    else
                    {
                        // Create new answer
                        var newAnswer = new SurveyAnswer
                        {
                            ResponseId = response.Id,
                            QuestionId = answerInput.QuestionId,
                            OptionId = answerInput.OptionId,
                            NumberValue = answerInput.NumberValue,
                            TextValue = answerInput.TextValue,
                            AnsweredAt = answerInput.AnsweredAt
                        };
                        await db.SurveyAnswers.AddAsync(newAnswer, cancellationToken);
                    }

                    processedCount++;
                }

                await db.SaveChangesAsync(cancellationToken);

                // 4. If submitted, finalize response
                var status = "InProgress";

                if (request.SubmittedAt.HasValue)
                {
                    // Validate required questions
                    var requiredQuestions = survey.Questions.Where(q => q.IsRequired).ToList();
                    var answeredQuestionIds = await db.SurveyAnswers
                        .Where(a => a.ResponseId == response.Id)
                        .Select(a => a.QuestionId)
                        .ToListAsync(cancellationToken);

                    var missingRequired = requiredQuestions
                        .Where(q => !answeredQuestionIds.Contains(q.Id))
                        .Select(q => $"Question {q.OrderNo}: {q.Prompt}")
                        .ToList();

                    if (missingRequired.Any())
                    {
                        validationErrors.AddRange(missingRequired.Select(m => $"Missing required: {m}"));
                        
                        await db.RollbackTransactionAsync(cancellationToken);
                        return new TakeSurveyResponse(
                            false,
                            "Cannot submit: Missing required questions",
                            new TakeSurveyData
                            {
                                ResponseId = response.Id,
                                Status = "InProgress",
                                AnsweredCount = processedCount,
                                TotalQuestions = survey.Questions.Count,
                                ValidationErrors = validationErrors
                            });
                    }

                    // Generate snapshot
                    var allAnswers = await db.SurveyAnswers
                        .Where(a => a.ResponseId == response.Id)
                        .Select(a => new
                        {
                            a.Id,
                            a.ResponseId,
                            a.QuestionId,
                            a.OptionId,
                            a.NumberValue,
                            a.TextValue,
                            a.AnsweredAt
                        })
                        .ToListAsync(cancellationToken);

                    var snapshotJson = JsonSerializer.Serialize(allAnswers, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    });

                    response.SubmittedAt = request.SubmittedAt.Value;
                    response.SnapshotJson = snapshotJson;
                    status = "Completed";

                    await db.SaveChangesAsync(cancellationToken);
                }

                await db.CommitTransactionAsync(cancellationToken);

                return new TakeSurveyResponse(
                    true,
                    status == "Completed" ? "Survey submitted successfully" : "Survey draft saved successfully",
                    new TakeSurveyData
                    {
                        ResponseId = response.Id,
                        Status = status,
                        AnsweredCount = processedCount,
                        TotalQuestions = survey.Questions.Count,
                        ValidationErrors = validationErrors.Any() ? validationErrors : null
                    });
            }
            catch (NotFoundException ex)
            {
                await db.RollbackTransactionAsync(cancellationToken);
                return new TakeSurveyResponse(false, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await db.RollbackTransactionAsync(cancellationToken);
                return new TakeSurveyResponse(false, ex.Message);
            }
            catch (Exception ex)
            {
                await db.RollbackTransactionAsync(cancellationToken);
                return new TakeSurveyResponse(false, $"Error processing survey: {ex.Message}");
            }
        }
    }
}