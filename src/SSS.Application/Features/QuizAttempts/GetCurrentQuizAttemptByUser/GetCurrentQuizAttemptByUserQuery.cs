using MediatR;

namespace SSS.Application.Features.QuizAttempts.GetCurrentQuizAttemptByUser
{
    public sealed record GetCurrentQuizAttemptByUserQuery(long ModuleId) 
        : IRequest<GetCurrentQuizAttemptByUserResult>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; } = null!;
    }
}
