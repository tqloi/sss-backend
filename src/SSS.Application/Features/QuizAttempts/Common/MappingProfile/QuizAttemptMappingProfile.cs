using AutoMapper;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAttempts.Common.MappingProfile
{
    public class QuizAttemptMappingProfile : Profile
    {
        public QuizAttemptMappingProfile()
        {
            CreateMap<QuizAttempt, QuizAttemptDto>();
        }
    }
}
