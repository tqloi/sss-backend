using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.External.AI.LLM;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Entities.AI;
using SSS.Domain.Enums;
using System.Text;
using System.Text.Json;

namespace SSS.Application.Features.AI.Chat.SendMessage
{
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, SendMessageResult>
    {
        private readonly IAppDbContext _sqlDb;
        private readonly IAiConversationRepository _conversationRepo;
        private readonly IAiChatMessageRepository _chatMessageRepo;
        private readonly ILlmRouter _llmRouter;

        private const int MaxHistoryMessages = 10;

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

            // 1. Resolve or Create Conversation (scoped to roadmap)
            AiConversation conversation;
            if (!string.IsNullOrEmpty(request.ConversationId))
            {
                conversation = await _conversationRepo.GetByIdAsync(request.ConversationId);
                if (conversation == null || conversation.UserId != userId)
                    throw new Exception("Conversation not found");
            }
            else
            {
                // Try to find existing active conversation for this user + roadmap
                conversation = await _conversationRepo.GetByUserAndRoadmapAsync(userId, request.RoadmapId);

                if (conversation == null)
                {
                    // Create new conversation for this roadmap
                    var roadmapTitle = await _sqlDb.Roadmaps
                        .Where(r => r.Id == request.RoadmapId)
                        .Select(r => r.Title)
                        .FirstOrDefaultAsync(cancellationToken);

                    conversation = new AiConversation
                    {
                        Id = Guid.NewGuid().ToString("N")[..24],
                        UserId = userId,
                        RoadmapId = request.RoadmapId,
                        Title = roadmapTitle ?? $"Roadmap {request.RoadmapId}",
                        CreatedAt = DateTime.UtcNow,
                        LastMessageAt = DateTime.UtcNow,
                        IsActive = true
                    };
                    await _conversationRepo.AddAsync(conversation);
                }
            }

            // 2. Build context from selected modules and tasks
            var contextBuilder = new StringBuilder();
            var hasModules = request.ModuleIds != null && request.ModuleIds.Count > 0;
            var hasTasks = request.TaskIds != null && request.TaskIds.Count > 0;

            if (hasModules)
            {
                var modules = await _sqlDb.StudyPlanModules
                    .Include(m => m.RoadmapNode)
                    .Include(m => m.Tasks)
                    .AsNoTracking()
                    .Where(m => request.ModuleIds!.Contains(m.Id))
                    .ToListAsync(cancellationToken);

                if (modules.Any())
                {
                    contextBuilder.AppendLine("[MODULES CONTEXT]");
                    foreach (var module in modules)
                    {
                        contextBuilder.AppendLine($"- Module: '{module.RoadmapNode?.Title ?? "Unknown"}' (ID: {module.Id}), Status: {module.Status}");
                        if (module.Tasks.Any())
                        {
                            foreach (var task in module.Tasks)
                            {
                                contextBuilder.AppendLine($"  - Task: '{task.Title}', Status: {task.Status}, Description: '{task.Description}'");
                            }
                        }
                    }
                }
            }

            if (hasTasks)
            {
                var tasks = await _sqlDb.TaskItems
                    .AsNoTracking()
                    .Where(t => request.TaskIds!.Contains(t.Id))
                    .ToListAsync(cancellationToken);

                if (tasks.Any())
                {
                    contextBuilder.AppendLine("[TASKS CONTEXT]");
                    foreach (var task in tasks)
                    {
                        contextBuilder.AppendLine($"- Task: '{task.Title}' (ID: {task.Id}), Status: {task.Status}, Description: '{task.Description}', EstimatedDuration: {task.EstimatedDurationSeconds}s");
                    }
                }
            }

            var contextInfo = contextBuilder.ToString().TrimEnd();

            // 3. Save user message with context
            var messageContext = (hasModules || hasTasks)
                ? new MessageContext
                {
                    ModuleIds = request.ModuleIds ?? new List<long>(),
                    TaskIds = request.TaskIds ?? new List<long>()
                }
                : null;

            var userMessage = new AiChatMessage
            {
                Id = Guid.NewGuid().ToString("N")[..24],
                ConversationId = conversation.Id,
                UserId = userId,
                Role = AiMessageRole.User,
                MessageContent = request.MessageContent,
                Context = messageContext,
                Timestamp = DateTime.UtcNow
            };
            await _chatMessageRepo.AddAsync(userMessage);

            // 4. Fetch recent chat history for context continuity
            var recentMessages = await _chatMessageRepo.GetByConversationIdAsync(conversation.Id);
            var historyMessages = recentMessages
                .OrderByDescending(m => m.Timestamp)
                .Skip(1) // skip current user message (just added)
                .Take(MaxHistoryMessages)
                .OrderBy(m => m.Timestamp)
                .ToList();

            // 5. Construct prompt with history and context
            var systemPrompt = """
                You are an intelligent Study Assistant helping a student with their learning roadmap.
                Be friendly, concise, and educational.
                If the student asks a question related to their current learning modules or tasks, use the provided context to guide your answer.
                Do NOT invent details about their tasks that aren't mentioned in the context.
                If conversation history is provided, use it to maintain continuity in the conversation.
                """;

            var userPromptBuilder = new StringBuilder();

            // Add chat history
            if (historyMessages.Any())
            {
                userPromptBuilder.AppendLine("[CONVERSATION HISTORY]");
                foreach (var msg in historyMessages)
                {
                    var role = msg.Role == AiMessageRole.User ? "User" : "Assistant";
                    userPromptBuilder.AppendLine($"{role}: {msg.MessageContent}");
                }
                userPromptBuilder.AppendLine();
            }

            // Add context
            if (!string.IsNullOrEmpty(contextInfo))
            {
                userPromptBuilder.AppendLine(contextInfo);
                userPromptBuilder.AppendLine();
            }

            userPromptBuilder.AppendLine($"[USER MESSAGE] {request.MessageContent}");

            // 6. Query LLM
            var llmChatProvider = _llmRouter.Resolve(LlmTask.SimpleChat);
            var aiResponseContent = await llmChatProvider.AskAsync(
                systemPrompt,
                userPromptBuilder.ToString(),
                cancellationToken);

            // 7. Save AI response
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

            // 8. Update conversation last touched
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
