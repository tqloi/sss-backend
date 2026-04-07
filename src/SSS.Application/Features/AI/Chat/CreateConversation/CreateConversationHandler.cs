using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Domain.Entities.AI;

namespace SSS.Application.Features.AI.Chat.CreateConversation
{
    public class CreateConversationHandler : IRequestHandler<CreateConversationCommand, CreateConversationResult>
    {
        private readonly IAppDbContext _sqlDb;
        private readonly IAiConversationRepository _conversationRepo;

        public CreateConversationHandler(
            IAppDbContext sqlDb,
            IAiConversationRepository conversationRepo)
        {
            _sqlDb = sqlDb;
            _conversationRepo = conversationRepo;
        }

        public async Task<CreateConversationResult> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var roadmapTitle = request.Title;

            // If no title provided, fetch from DB
            if (string.IsNullOrWhiteSpace(roadmapTitle))
            {
                var roadmap = await _sqlDb.Roadmaps
                    .AsNoTracking()
                    .Where(r => r.Id == request.RoadmapId)
                    .Select(r => r.Title)
                    .FirstOrDefaultAsync(cancellationToken);

                if (roadmap == null)
                    throw new NotFoundException($"Roadmap with ID {request.RoadmapId} was not found.");

                roadmapTitle = roadmap;
            }

            // Optional: When creating a new conversation for a roadmap, we typically 
            // set other active ones for this roadmap to inactive if we want only 1 active at a time.
            // Assuming the client uses `ConversationId` directly, we might not strictly need to deactivate others, 
            // but let's just create it as active.
            
            var conversation = new AiConversation
            {
                Id = Guid.NewGuid().ToString("N")[..24],
                UserId = request.UserId,
                RoadmapId = request.RoadmapId,
                Title = roadmapTitle,
                CreatedAt = DateTime.UtcNow,
                LastMessageAt = DateTime.UtcNow,
                IsActive = true
            };

            await _conversationRepo.AddAsync(conversation);

            return new CreateConversationResult
            {
                Success = true,
                Message = "Conversation created successfully.",
                ConversationId = conversation.Id,
                Title = conversation.Title
            };
        }
    }
}
