using AutoMapper;
using SSS.Application.Features.Surveys.SurveyAnswers.CreateAnswerByResponse;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.CreateSurveyFieldSemantic;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption;
using SSS.Application.Features.Surveys.SurveyQuestions.CreateSurveyQuestion;
using SSS.Application.Features.Surveys.SurveyResponses.CreateSurveyResponse;
using SSS.Application.Features.Surveys.Surveys.CreateSurvey;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.CreateSurveyTriggerMapping;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Common
{
    public class SurveyMappingProfile : Profile
    {
        public SurveyMappingProfile() 
        {
            CreateMap<Survey, SurveyDto>();
            CreateMap<CreateSurveyCommand, Survey>();

            CreateMap<CreateSurveyQuestionCommand, SurveyQuestion>();
            CreateMap<SurveyQuestion, SurveyQuestionDto>();

            
            CreateMap<CreateSurveyQuestionOptionCommand, SurveyQuestionOption>();
            CreateMap<SurveyQuestionOption, SurveyQuestionOptionDto>();

            CreateMap<CreateSurveyResponseCommand, SurveyResponse>();
            CreateMap<SurveyResponse, SurveyResponseDto>();

            CreateMap<CreateAnswerByResponseCommand, SurveyAnswer>();
            CreateMap<SurveyAnswer, SurveyAnswerDto>();

            CreateMap<CreateSurveyFieldSemanticCommand, SurveyFieldSemantic>();
            CreateMap<SurveyFieldSemantic, SurveyFieldSenmaticDto>();

            CreateMap<CreateSurveyTriggerMappingCommand, SurveyTriggerMapping>();
            CreateMap<SurveyTriggerMapping, SurveyTriggerMappingDto>(); 




        }

        
    }
}
