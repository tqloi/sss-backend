using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.Background;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Application.Common.Exceptions;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Enums;
using SurveyCodeConstants = SSS.Domain.Constants.SurveyCodes;
using SurveyTriggerTypeCodes = SSS.Domain.Constants.SurveyTriggerTypes;
using System.Text.Json;

namespace SSS.Application.Features.Surveys.Surveys.TakeSurvey
{
    public class TakeSurveyHandler(
        IAppDbContext db,
        ISurveyJobDispatcher jobDispatcher,
        INotificationService notificationService,
        ILogger<TakeSurveyHandler> logger) 
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

                var questionsById = survey.Questions.ToDictionary(q => q.Id);

                var optionIdsToValidate = request.Answers
                    .Where(a => a.OptionId.HasValue)
                    .Select(a => a.OptionId!.Value)
                    .Distinct()
                    .ToList();

                var optionQuestionIdByOptionId = optionIdsToValidate.Count == 0
                    ? new Dictionary<long, long>()
                    : await db.SurveyQuestionOptions
                        .AsNoTracking()
                        .Where(o => optionIdsToValidate.Contains(o.Id))
                        .Select(o => new { o.Id, o.QuestionId })
                        .ToDictionaryAsync(x => x.Id, x => x.QuestionId, cancellationToken);

                var existingAnswers = response.Id == 0
                    ? new List<SurveyAnswer>()
                    : await db.SurveyAnswers
                        .Where(a => a.ResponseId == response.Id)
                        .ToListAsync(cancellationToken);

                var answersByQuestionId = existingAnswers
                    .GroupBy(a => a.QuestionId)
                    .ToDictionary(g => g.Key, g => g.OrderByDescending(a => a.AnsweredAt).First());

                var answersByQuestionOption = existingAnswers
                    .Where(a => a.OptionId.HasValue)
                    .GroupBy(a => new { a.QuestionId, OptionId = a.OptionId!.Value })
                    .ToDictionary(
                        g => (g.Key.QuestionId, g.Key.OptionId),
                        g => g.OrderByDescending(a => a.AnsweredAt).First());

                foreach (var answerInput in request.Answers)
                {
                    // Validate question exists in survey
                    if (!questionsById.ContainsKey(answerInput.QuestionId))
                    {
                        validationErrors.Add($"Question {answerInput.QuestionId} not found in survey");
                        continue;
                    }

                    // Validate option if provided
                    if (answerInput.OptionId.HasValue)
                    {
                        if (!optionQuestionIdByOptionId.TryGetValue(answerInput.OptionId.Value, out var optionQuestionId)
                            || optionQuestionId != answerInput.QuestionId)
                        {
                            validationErrors.Add($"Option {answerInput.OptionId} not found for question {answerInput.QuestionId}");
                            continue;
                        }
                    }

                    var question = questionsById[answerInput.QuestionId];

                    if (question.Type == SurveyQuestionType.MultipleChoice)
                    {
                        if (!answerInput.OptionId.HasValue)
                        {
                            validationErrors.Add(
                                $"Option is required for multiple choice question {answerInput.QuestionId}");
                            continue;
                        }

                        var optionKey = (answerInput.QuestionId, answerInput.OptionId.Value);
                        if (answersByQuestionOption.TryGetValue(optionKey, out var existingAnswer))
                        {
                            existingAnswer.NumberValue = answerInput.NumberValue;
                            existingAnswer.TextValue = answerInput.TextValue;
                            existingAnswer.AnsweredAt = answerInput.AnsweredAt;
                        }
                        else
                        {
                            var newAnswer = new SurveyAnswer
                            {
                                Response = response,
                                QuestionId = answerInput.QuestionId,
                                OptionId = answerInput.OptionId,
                                NumberValue = answerInput.NumberValue,
                                TextValue = answerInput.TextValue,
                                AnsweredAt = answerInput.AnsweredAt
                            };
                            await db.SurveyAnswers.AddAsync(newAnswer, cancellationToken);
                            existingAnswers.Add(newAnswer);
                            answersByQuestionOption[optionKey] = newAnswer;
                        }
                    }
                    else
                    {
                        if (answersByQuestionId.TryGetValue(answerInput.QuestionId, out var existingAnswer))
                        {
                            existingAnswer.OptionId = answerInput.OptionId;
                            existingAnswer.NumberValue = answerInput.NumberValue;
                            existingAnswer.TextValue = answerInput.TextValue;
                            existingAnswer.AnsweredAt = answerInput.AnsweredAt;
                        }
                        else
                        {
                            var newAnswer = new SurveyAnswer
                            {
                                Response = response,
                                QuestionId = answerInput.QuestionId,
                                OptionId = answerInput.OptionId,
                                NumberValue = answerInput.NumberValue,
                                TextValue = answerInput.TextValue,
                                AnsweredAt = answerInput.AnsweredAt
                            };
                            await db.SurveyAnswers.AddAsync(newAnswer, cancellationToken);
                            existingAnswers.Add(newAnswer);
                            answersByQuestionId[answerInput.QuestionId] = newAnswer;
                        }
                    }

                    processedCount++;
                }

                await db.SaveChangesAsync(cancellationToken);

                // 4. If submitted, finalize response
                var status = "InProgress";

                if (request.SubmittedAt.HasValue)
                {
                    var shouldSendFirstOnRegisterCompletionNotification = false;

                    // Validate required questions
                    var requiredQuestions = survey.Questions.Where(q => q.IsRequired).ToList();
                    var answeredQuestionIds = existingAnswers
                        .Select(a => a.QuestionId)
                        .ToHashSet();

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
                    var allAnswers = existingAnswers
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
                        .ToList();

                    var snapshotJson = JsonSerializer.Serialize(allAnswers, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                    });

                    response.SubmittedAt = request.SubmittedAt.Value;
                    response.SnapshotJson = snapshotJson;
                    status = "Completed";

                    // Send a congratulation notification only for the first completion
                    // of a survey mapped to ON_REGISTER trigger.
                    var isOnRegisterSurvey = await db.SurveyTriggerMappings
                        .AsNoTracking()
                        .AnyAsync(m =>
                            m.SurveyId == request.SurveyId
                            && m.IsActive
                            && m.TriggerType == SurveyTriggerTypeCodes.OnRegister,
                            cancellationToken);

                    if (isOnRegisterSurvey)
                    {
                        var completedCount = await db.SurveyResponses
                            .AsNoTracking()
                            .Where(r =>
                                r.UserId == request.UserId
                                && r.SurveyId == request.SurveyId
                                && r.SubmittedAt != null
                                && r.Id != response.Id)
                            .CountAsync(cancellationToken);

                        shouldSendFirstOnRegisterCompletionNotification = completedCount == 0;
                    }

                    await db.SaveChangesAsync(cancellationToken);

                    if (shouldSendFirstOnRegisterCompletionNotification)
                    {
                        // Notification failure must not block survey submission.
                        try
                        {
                            await notificationService.CreateAndDispatchAsync(
                                request.UserId,
                                "Thank you for completing the first survey.",
                                "Your academic record will be better optimized.",
                                NotificationType.Achievement,
                                ct: cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning(
                                ex,
                                "[TakeSurvey] Failed to send first ON_REGISTER completion notification. UserId={UserId}, SurveyId={SurveyId}",
                                request.UserId,
                                request.SurveyId);
                        }
                    }

                    // Dispatch background AI analysis job based on survey type
                    if (survey.Code == SurveyCodeConstants.LearningBehavior)
                    {
                        jobDispatcher.DispatchBehaviorAnalysis(response.Id);
                    }
                    else if (survey.Code == SurveyCodeConstants.RoadmapLearningTarget)
                    {
                        if (!request.RoadmapId.HasValue)
                            throw new InvalidOperationException(
                                "RoadmapId is required when submitting a ROADMAP_LEARNING_TARGET survey.");

                        jobDispatcher.DispatchTargetAnalysis(response.Id, request.RoadmapId.Value);
                    }
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