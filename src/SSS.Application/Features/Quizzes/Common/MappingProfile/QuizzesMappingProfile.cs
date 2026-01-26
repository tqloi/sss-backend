using AutoMapper;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.Quizzes.Common.MappingProfile
{
    public class QuizzesMappingProfile : Profile
    {
        public QuizzesMappingProfile()
        {
            CreateMap<Quiz, CreateQuizDto>();
            CreateMap<CreateQuizDto, Quiz>();

            CreateMap<Quiz, UpdateQuizDto>();
            CreateMap<UpdateQuizDto, Quiz>();
        }
    }
}
