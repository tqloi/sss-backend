using MediatR;

namespace SSS.Application.Features.UserManagement.ActivateUser
{
    public sealed class ActivateUserCommand : IRequest<bool>
    {
        public string Id { get; set; } = null!;
    }
}
