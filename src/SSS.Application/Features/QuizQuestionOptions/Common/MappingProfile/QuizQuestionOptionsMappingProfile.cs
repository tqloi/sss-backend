using AutoMapper;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestionOptions.Common.MappingProfile
{
    public class QuizQuestionOptionsMappingProfile : Profile
    {
        public QuizQuestionOptionsMappingProfile()
        {
            CreateMap<CreateQuizQuestionOptionDto, QuizQuestionOption>();
            CreateMap<QuizQuestionOption, QuizQuestionOptionDto>();
            CreateMap<UpdateQuizQuestionOptionDto, QuizQuestionOption>();
            CreateMap<QuizQuestionOption, UpdateQuizQuestionOptionDto>();
        }
    }
}
