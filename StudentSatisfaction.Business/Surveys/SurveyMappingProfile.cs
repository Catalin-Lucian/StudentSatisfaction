using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Models.Comments;
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
            CreateMap<SurveyModel, Survey>(); /////

            //CreateMap<CreateCommentModel, Comment>()
            //    .ForMember(dest => dest.)
        }
    }
}
