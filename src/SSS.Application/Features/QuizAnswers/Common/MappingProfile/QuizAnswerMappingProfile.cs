using AutoMapper;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAnswers.Common.MappingProfile
{
    public class QuizAnswerMappingProfile : Profile
    {
        public QuizAnswerMappingProfile()
        {
            CreateMap<UpdateQuizAnswerDto, QuizAnswer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<QuizAnswer, UpdateQuizAnswerDto>();
            CreateMap<QuizAnswer, CreateQuizAnswerDto>();
            CreateMap<CreateQuizAnswerDto, QuizAnswer>();
        }
    }
}
