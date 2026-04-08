using MediatR;

namespace SSS.Application.Features.UserManagement.UnassignSubjectFromContentManager
{
    public sealed class UnassignSubjectFromContentManagerCommand : IRequest<bool>
    {
        public string ContentManagerId { get; set; } = null!;
        public long? SubjectId { get; set; }
    }
}
