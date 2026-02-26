using AutoMapper;
using SSS.Domain.Entities.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.Common.MappingProfile
{
    public class UserLearningMappingProfile : Profile
    {
        public UserLearningMappingProfile()
        {
            CreateMap<UserLearningTarget, UserLearningTargetDto>();
            CreateMap<UserLearningTargetDto, UserLearningTarget>();

            CreateMap<UserLearningBehavior, UserLearningBehaviorDto>();
            CreateMap<UserLearningBehaviorDto, UserLearningBehavior>();
        }
    }
}
