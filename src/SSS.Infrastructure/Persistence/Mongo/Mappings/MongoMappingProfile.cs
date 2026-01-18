using AutoMapper;
using SSS.Domain.Entities.AI;
using SSS.Domain.Entities.Tracking;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo.Mappings
{
    public class MongoMappingProfile : Profile
    {
        public MongoMappingProfile()
        {
            CreateMap<AiConversation, AiConversationDocument>().ReverseMap();
            CreateMap<AiChatMessage, AiChatMessageDocument>().ReverseMap();
            CreateMap<AiContextLog, AiContextLogDocument>().ReverseMap();
            CreateMap<AiRecommendation, AiRecommendationDocument>().ReverseMap();
            CreateMap<StudyEvent, StudyEventDocument>().ReverseMap();
        }
    }
}
