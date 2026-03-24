using MediatR;

namespace SSS.Application.Features.UserManagement.DeactivateUser
{
    public sealed class DeactivateUserCommand : IRequest<bool>
    {
        public string Id { get; set; } = null!;
    }
}
