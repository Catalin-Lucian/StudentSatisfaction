using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Users
{
    public sealed class UserService : IUsersService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IMapper _mapper;


        public UserService(ISurveyRepository surveyRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
        }

        public async Task<UserModel> Add(Guid surveyId, CreateUserModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            var user = _mapper.Map<User>(model);
            survey.Users.Add(user);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();

            return _mapper.Map<UserModel>(user);
        }

        public async Task Delete(Guid surveyId, Guid userId)
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

        public async Task<IEnumerable<UserModel>> Get(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<IEnumerable<UserModel>>(survey.Users);
        }
    }
}
