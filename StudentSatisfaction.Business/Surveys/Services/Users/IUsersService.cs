using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Models.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSatisfaction.Business.Surveys.Models.Questions;

namespace StudentSatisfaction.Business.Surveys.Services.Users
{
    public interface IUsersService
    {
        //manage UsersData table
        IEnumerable<UserModel> GetAllUsers();
        Task<UserModel> GetUserById(Guid userId);
        Task<UserModel> Create(CreateUserModel model);
        Task<UserModel> Create(UserModel model);
        Task Delete(Guid userId);
        Task DeleteCredentials(Guid userId);
        Task Update(Guid userId, UpdateUserModel model);


        // manage Users that completed a Survey
        Task<IEnumerable<UserModel>> GetAllUsersFromSurvey(Guid surveyId);
        //varianta asta e folosita in SurveyController
        Task<UserModel> AddUserToSurvey(Guid surveyId, Guid userId);
        Task<UserModel> AddUserToSurvey(Guid surveyId, CreateUserModel model);
        Task DeleteUserFromSurvey(Guid surveyId, Guid userId);
        Task<UserModel> GetUserFromSurvey(Guid surveyId, Guid userId);


        Task<IEnumerable<SurveyModel>> GetAnsweredSurveys(Guid userId);
        Task<IEnumerable<SurveyModel>> GetNotAnsweredSurveys(Guid userId);


        Task<IEnumerable<QuestionModel>> GetAnsweredQuestions(Guid userId);
    }
}
