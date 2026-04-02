using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Content.LearningSubject.GetByContentManager
{
    public sealed class GetSubjectByContentManagerQuery : IRequest<GetSubjectByContentManagerResult>
    {
        [JsonIgnore]
        public string ManagerId { get; set; } = null!;
    }
}
