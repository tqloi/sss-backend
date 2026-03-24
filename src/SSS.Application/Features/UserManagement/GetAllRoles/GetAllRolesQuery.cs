using MediatR;

namespace SSS.Application.Features.UserManagement.GetAllRoles
{
    public sealed record GetAllRolesQuery : IRequest<GetAllRolesResponse>;
}
