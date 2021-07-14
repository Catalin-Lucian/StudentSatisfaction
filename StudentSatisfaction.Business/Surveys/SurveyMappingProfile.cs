using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Models.Comments;
using StudentSatisfaction.Business.Surveys.Models.Questions;
using StudentSatisfaction.Business.Surveys.Models.Topics;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys
{
    public sealed class SurveyMappingProfile: Profile
    {
        public SurveyMappingProfile()
        {
            CreateMap<Survey, SurveyModel>();
            CreateMap<SurveyModel, Survey>(); 

            //genereaza automat un Guid pt. noul Survey
            CreateMap<CreateSurveyModel, Survey>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<Survey, UpdateSurveyModel>();
            CreateMap<UpdateSurveyModel, Survey>();


            CreateMap<Question, QuestionModel>();
            CreateMap<CreateQuestionModel, Question>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<Question, UpdateQuestionModel>();
            CreateMap<UpdateQuestionModel, Question>();

            CreateMap<Topic, TopicModel>();
            CreateMap<TopicModel, Topic>();
        }
    }
}
