namespace SSS.Application.Features.UserManagement.Common
{
    public sealed class AssignedSubjectDto
    {
        public long SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
    }

    public sealed class UserDto
    {
        public string Id { get; set; } = default!;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public List<string> RoleNames { get; set; } = new();
        public long? AssignedSubjectId { get; set; }
        public string? AssignedSubjectName { get; set; }
        public List<AssignedSubjectDto> AssignedSubjects { get; set; } = new();
    }
}
