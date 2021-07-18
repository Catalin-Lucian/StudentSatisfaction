using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Models.Comments;
using StudentSatisfaction.Business.Surveys.Models.Notifications;
using StudentSatisfaction.Business.Surveys.Models.Questions;
using StudentSatisfaction.Business.Surveys.Models.Ratings;
using StudentSatisfaction.Business.Surveys.Models.SubmittedQuestions;
using StudentSatisfaction.Business.Surveys.Models.Topics;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
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
            CreateMap<UpdateTopicModel, Topic>();
            CreateMap<CreateTopicModel, Topic>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())); 


            CreateMap<User, UserModel>();
            CreateMap<UpdateUserModel, User>();
            CreateMap<CreateUserModel, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));


            CreateMap<Notification, NotificationModel>();
            CreateMap<NotificationModel, Notification>();
            CreateMap<UpdateNotificationModel, Notification>();
            CreateMap<CreateNotificationModel, Notification>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));


            CreateMap<Comment, CommentModel>();
            CreateMap<Comment, UpdateCommentModel>();
            CreateMap<UpdateCommentModel, Comment>();
            CreateMap<CreateCommentModel, Comment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));


            CreateMap<SubmittedQuestion, SubmittedQuestionsModel>();
            CreateMap<UpdateSubmittedQuestionModel, SubmittedQuestion>();
            CreateMap<CreateSubmittedQuestionModel, SubmittedQuestion>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));


            CreateMap<CreateRatingModel, Rating>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<Rating, RatingModel>();
            CreateMap<UpdateRatingModel, Rating>();
        }
    }
}
