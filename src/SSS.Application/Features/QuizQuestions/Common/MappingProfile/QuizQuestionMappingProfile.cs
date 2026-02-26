using AutoMapper;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.QuizQuestions.Common.MappingProfile
{
    public class QuizQuestionMappingProfile : Profile
    {
        public QuizQuestionMappingProfile() 
        {
            CreateMap<CreateQuizQuestionDto, QuizQuestion>();
            CreateMap<QuizQuestion, CreateQuizQuestionDto>();
            CreateMap<UpdateQuizQuestionDto, QuizQuestion>();
            CreateMap<QuizQuestion, UpdateQuizQuestionDto>();
                CreateMap<QuizQuestion, QuizQuestionDto>();
                CreateMap<QuizQuestionDto, QuizQuestion>();
        }
    }
}
