using AutoMapper;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.QuizAnswers.Common.MappingProfile
{
    public class QuizAnswerMappingProfile : Profile
    {
        public QuizAnswerMappingProfile()
        {
                CreateMap<CreateQuizAnswerDto, QuizAnswer>();
                CreateMap<QuizAnswer, CreateQuizAnswerDto>();
                CreateMap<QuizAnswer, QuizAnswerDto>();
        }
    }
}
