using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Users
{
    public sealed class UserService : IUsersService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(ISurveyRepository surveyRepository, IUserRepository userRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }


        //Manage Users
        public IEnumerable<UserModel> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<UserModel>>(_userRepository.GetAll());
        }

        public async Task<UserModel> GetUserById(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> Create(CreateUserModel model)
        {
            var user = _mapper.Map<User>(model);
            await _userRepository.Create(user);
            await _userRepository.SaveChanges();

            return _mapper.Map<UserModel>(user);
        }

        public async Task Delete(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);

            _userRepository.Delete(user);
            await _userRepository.SaveChanges();
        }

        public async Task Update(Guid userId, UpdateUserModel model)
        {
            var user = await _userRepository.GetUserById(userId);
            _mapper.Map(model, user);

            _userRepository.Update(user);
            await _userRepository.SaveChanges();
        }

        public async Task<IEnumerable<SurveyModel>> GetAnsweredSurveys(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var answeredSurveys = user.Surveys;

            var surveys = _mapper.Map<IEnumerable<SurveyModel>>(answeredSurveys);

            return surveys;
        }

        public async Task<IEnumerable<SurveyModel>> GetNotAnsweredSurveys(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var answeredSurveys = user.Surveys;

            var surveys = _surveyRepository.GetAll()
                .Where(s => !answeredSurveys.Contains(s))
                .Where(s => s.StartDate < DateTime.Now && s.EndDate >DateTime.Now);
            
            var notAnsweredSurveys = _mapper.Map<IEnumerable<SurveyModel>>(surveys);

            return notAnsweredSurveys;
        }

        //Manage Users from Survey

        public async Task<UserModel> GetUserFromSurvey(Guid surveyId, Guid userId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var user = survey.Users.FirstOrDefault(u => u.Id == userId);

            return _mapper.Map<UserModel>(user);
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersFromSurvey(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<IEnumerable<UserModel>>(survey.Users);
        }

        //Overload 
        public async Task<UserModel> AddUserToSurvey(Guid surveyId, CreateUserModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            var user = _mapper.Map<User>(model);
            survey.Users.Add(user);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();

            return _mapper.Map<UserModel>(user);
        }

        //Overload --> folosita in SurveyController
        public async Task<UserModel> AddUserToSurvey(Guid surveyId, Guid userId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var user = await _userRepository.GetUserById(userId);

            //adaug user-ul primit doar daca daca survey-ul
            //nu contine deja user-ul respectiv
            if (!survey.Users.Contains(user))
            {
                survey.Users.Add(user);

                _surveyRepository.Update(survey);
                await _surveyRepository.SaveChanges();
            }

            return _mapper.Map<UserModel>(user);
        }


        public async Task DeleteUserFromSurvey(Guid surveyId, Guid userId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var userToRemove = survey.Users.FirstOrDefault(u => u.Id == userId);

            if(userToRemove != null)
            {
                survey.Users.Remove(userToRemove);
            }

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();
        }
    }
}
