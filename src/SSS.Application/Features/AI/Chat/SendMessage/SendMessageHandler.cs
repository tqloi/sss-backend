using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.External.AI.LLM;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Entities.AI;
using SSS.Domain.Enums;
using System.Text.Json;

namespace SSS.Application.Features.AI.Chat.SendMessage
{
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, SendMessageResult>
    {
        private readonly IAppDbContext _sqlDb;
        private readonly IAiConversationRepository _conversationRepo;
        private readonly IAiChatMessageRepository _chatMessageRepo;
        private readonly ILlmRouter _llmRouter;

        public SendMessageHandler(
            IAppDbContext sqlDb,
            IAiConversationRepository conversationRepo,
            IAiChatMessageRepository chatMessageRepo,
            ILlmRouter llmRouter)
        {
            _sqlDb = sqlDb;
            _conversationRepo = conversationRepo;
            _chatMessageRepo = chatMessageRepo;
            _llmRouter = llmRouter;
        }

        public async Task<SendMessageResult> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            // 1. Resolve or Create Conversation
            AiConversation conversation = null!;
            if (!string.IsNullOrEmpty(request.ConversationId))
            {
                conversation = await _conversationRepo.GetByIdAsync(request.ConversationId);
                if (conversation == null || conversation.UserId != userId)
                    throw new Exception("Conversation not found");
            }
            else
            {
                conversation = new AiConversation
                {
                    Id = Guid.NewGuid().ToString("N")[..24],
                    UserId = userId,
                    Title = request.MessageContent.Length > 30 ? request.MessageContent.Substring(0, 30) + "..." : request.MessageContent,
                    RelatedType = request.RelatedType,
                    RelatedId = request.RelatedId,
                    CreatedAt = DateTime.UtcNow,
                    LastMessageAt = DateTime.UtcNow,
                    IsActive = true
                };
                await _conversationRepo.AddAsync(conversation);
            }

            // 2. Fetch context from SQL DB if provided
            string contextInfo = string.Empty;
            if (request.RelatedType.HasValue && !string.IsNullOrEmpty(request.RelatedId))
            {
                var relatedIdLong = long.TryParse(request.RelatedId, out var l) ? l : 0;
                var relatedIdInt = int.TryParse(request.RelatedId, out var i) ? i : 0;
                
                if (request.RelatedType == RelatedEntityType.Module)
                {
                    var module = await _sqlDb.StudyPlanModules
                        .Include(m => m.RoadmapNode)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Id == relatedIdInt, cancellationToken);
                    if (module != null)
                    {
                        var options = new JsonSerializerOptions
                        {
                            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                        };
                        contextInfo = $"User is asking about a StudyPlanModule titled '{module.RoadmapNode?.Title}'. Additional module info: {JsonSerializer.Serialize(module, options)}";
                    }
                }
                else if (request.RelatedType == RelatedEntityType.Task)
                {
                    var task = await _sqlDb.TaskItems
                        .AsNoTracking()
                        .FirstOrDefaultAsync(t => t.Id == relatedIdLong, cancellationToken);
                    if (task != null)
                    {
                        contextInfo = $"User is asking about a TaskItem titled '{task.Title}'. Description: '{task.Description}'. Status: {task.Status}.";
                    }
                }
            }

            // 3. Keep user message
            var userMessage = new AiChatMessage
            {
                Id = Guid.NewGuid().ToString("N")[..24],
                ConversationId = conversation.Id,
                UserId = userId,
                Role = AiMessageRole.User,
                MessageContent = request.MessageContent,
                Context = string.IsNullOrEmpty(contextInfo) ? null : contextInfo,
                Timestamp = DateTime.UtcNow
            };
            await _chatMessageRepo.AddAsync(userMessage);

            // 4. Construct Prompt
            var systemPrompt = """
You are an intelligent Study Assistant helping a student.
Be friendly, concise, and educational.
If the student asks a question related to their current learning module or task, use the provided context to guide your answer.
Do NOT invent details about their tasks that aren't mentioned in the context.
""";
            var userPrompt = request.MessageContent;
            if (!string.IsNullOrEmpty(contextInfo))
            {
                userPrompt = $"[CONTEXT] {contextInfo}\n[USER MESSAGE] {userPrompt}";
            }

            // 5. Query LLM (Gemini)
            var llmChatProvider = _llmRouter.Resolve(LlmTask.SimpleChat);
            var aiResponseContent = await llmChatProvider.AskAsync(systemPrompt, userPrompt, cancellationToken);

            // 6. Keep AI message
            var aiMessage = new AiChatMessage
            {
                Id = Guid.NewGuid().ToString("N")[..24],
                ConversationId = conversation.Id,
                UserId = userId,
                Role = AiMessageRole.System,
                MessageContent = aiResponseContent,
                Timestamp = DateTime.UtcNow
            };
            await _chatMessageRepo.AddAsync(aiMessage);

            // 7. Update conversation last touched
            conversation.LastMessageAt = DateTime.UtcNow;
            await _conversationRepo.UpdateAsync(conversation);

            return new SendMessageResult
            {
                ConversationId = conversation.Id,
                MessageId = aiMessage.Id,
                AiResponse = aiMessage.MessageContent,
                Timestamp = aiMessage.Timestamp
            };
        }
    }
}
