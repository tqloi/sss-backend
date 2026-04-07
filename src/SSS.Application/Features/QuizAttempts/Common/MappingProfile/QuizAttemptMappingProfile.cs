using AutoMapper;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.QuizAttempts.Common.MappingProfile
{
    public class QuizAttemptMappingProfile : Profile
    {
        public QuizAttemptMappingProfile()
        {
            CreateMap<QuizAttempt, QuizAttemptDto>();
            CreateMap<CreateQuizAttemptDto, QuizAttempt>();
            CreateMap<SubmitQuizAttempDto, QuizAttempt>();
        }
    }
}
