using MediatR;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Application.Common.Exceptions;

namespace SSS.Application.Features.AI.Chat.DeleteConversation
{
    public class DeleteConversationHandler : IRequestHandler<DeleteConversationCommand, DeleteConversationResult>
    {
        private readonly IAiConversationRepository _conversationRepo;
        private readonly IAiChatMessageRepository _chatMessageRepo;

        public DeleteConversationHandler(
            IAiConversationRepository conversationRepo,
            IAiChatMessageRepository chatMessageRepo)
        {
            _conversationRepo = conversationRepo;
            _chatMessageRepo = chatMessageRepo;
        }

        public async Task<DeleteConversationResult> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
        {
            var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId);
            if (conversation == null)
                throw new NotFoundException($"Conversation with ID {request.ConversationId} was not found.");

            if (!string.Equals(conversation.UserId, request.UserId, StringComparison.Ordinal))
                throw new ForbiddenException("You do not have permission to delete this conversation.");

            await _chatMessageRepo.DeleteByConversationIdAsync(request.ConversationId);
            await _conversationRepo.DeleteAsync(request.ConversationId);

            return new DeleteConversationResult
            {
                Success = true,
                Message = "Conversation deleted successfully."
            };
        }
    }
}