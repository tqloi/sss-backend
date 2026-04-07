using AutoMapper;
using SSS.Domain.Entities.Assessment;
using System.Text.Json;

namespace SSS.Application.Features.QuizAnswers.Common.MappingProfile
{
    public class QuizAnswerMappingProfile : Profile
    {
        public QuizAnswerMappingProfile()
        {
                CreateMap<CreateQuizAnswerDto, QuizAnswer>();
                CreateMap<QuizAnswer, CreateQuizAnswerDto>();
                CreateMap<QuizAnswer, QuizAnswerDto>()
                    .ForMember(dest => dest.OptionIds, opt => opt.MapFrom(src => ParseOptionIds(src.TextValue)));
                CreateMap<UpdateQuizAnswerDto, QuizAnswer>();
                CreateMap<QuizAnswer, UpdateQuizAnswerDto>()
                    .ForMember(dest => dest.OptionIds, opt => opt.MapFrom(src => ParseOptionIds(src.TextValue)));
        }

        private static List<long> ParseOptionIds(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new List<long>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<long>>(value) ?? new List<long>();
            }
            catch
            {
                return new List<long>();
            }
        }
    }
}
