using MediatR;

namespace SSS.Application.Features.UserManagement.AssignSubjectToContentManager
{
    public sealed class AssignSubjectToContentManagerCommand : IRequest<bool>
    {
        public string ContentManagerId { get; set; } = null!;
        public long SubjectId { get; set; }
        public string? AssignedBy { get; set; }
    }
}
