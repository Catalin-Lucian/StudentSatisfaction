using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models.Questions;
using StudentSatisfaction.Business.Surveys.Models.SubmittedQuestions;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.SubmittedQuestions;
using StudentSatisfaction.Persistence.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.SubmittedQuestions
{
    public sealed class SubmittedQuestionService : ISubmittedQuestionsService
    {
        private readonly ISubmittedQuestionRepository _submittedQuestionRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public SubmittedQuestionService(ISurveyRepository surveyRepository, IUserRepository userRepository, ISubmittedQuestionRepository submittedQuestionRepository, IMapper mapper)
        {
            _submittedQuestionRepository = submittedQuestionRepository;
            _surveyRepository = surveyRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubmittedQuestionsModel>> GetAllFromSurvey(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<IEnumerable<SubmittedQuestionsModel>>(survey.SubmittedQuestions);
        }


        public async Task<IEnumerable<SubmittedQuestionsModel>> GetAllFromUser(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);

            return _mapper.Map<IEnumerable<SubmittedQuestionsModel>>(user.SubmittedQuestions);
        }

        public async Task<SubmittedQuestionsModel> GetQuestionFromSurvey(Guid surveyId, Guid questionId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var question = survey.SubmittedQuestions.FirstOrDefault(c => c.Id == questionId);

            return _mapper.Map<SubmittedQuestionsModel>(question);
        }

        public async Task<SubmittedQuestionsModel> GetQuestionFromUser(Guid userId, Guid questionId)
        {
            var user = await _userRepository.GetUserById(userId);
            var question = user.SubmittedQuestions.FirstOrDefault(c => c.Id == questionId);

            return _mapper.Map<SubmittedQuestionsModel>(question);
        }

        public async Task<SubmittedQuestionsModel> Add(Guid surveyId, Guid userId, CreateSubmittedQuestionModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var user = await _userRepository.GetUserById(userId);
            var submittedQuestion = _mapper.Map<SubmittedQuestion>(model);

            //creez noua SubmittedQuestion
            await _submittedQuestionRepository.Create(submittedQuestion);
            await _submittedQuestionRepository.SaveChanges();

            survey.SubmittedQuestions.Add(submittedQuestion);
            user.SubmittedQuestions.Add(submittedQuestion);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();

            _userRepository.Update(user);
            await _userRepository.SaveChanges();

            return _mapper.Map<SubmittedQuestionsModel>(submittedQuestion);
        }

        public async Task Update(Guid surveyId, Guid submittedQuestionId, UpdateSubmittedQuestionModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var submittedQuestion = survey.SubmittedQuestions.FirstOrDefault(c => c.Id == submittedQuestionId);

            _mapper.Map(model, submittedQuestion);
            _surveyRepository.Update(survey);

            await _surveyRepository.SaveChanges();
        }

        public async Task DeleteFromSurvey(Guid surveyId, Guid submittedQuestionId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var submittedQuestionToRemove = survey.SubmittedQuestions.FirstOrDefault(q => q.Id == submittedQuestionId);

            if(submittedQuestionToRemove != null)
            {
                survey.SubmittedQuestions.Remove(submittedQuestionToRemove);
            }

            _surveyRepository.Update(survey);

            await _surveyRepository.SaveChanges();
        }

        public async Task DeleteFromUser(Guid userId, Guid submittedQuestionId)
        {
            var user = await _userRepository.GetUserById(userId);
            var submittedQuestionToRemove = user.SubmittedQuestions.FirstOrDefault(q => q.Id == submittedQuestionId);

            if (submittedQuestionToRemove != null)
            {
                user.SubmittedQuestions.Remove(submittedQuestionToRemove);
            }

            _userRepository.Update(user);

            await _userRepository.SaveChanges();
        }
    }
}
