using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models.Ratings;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.Questions;
using StudentSatisfaction.Persistence.Repositories.Ratings;
using StudentSatisfaction.Persistence.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Ratings
{
    public sealed class RatingService : IRatingService
    {
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public RatingService(IUserRepository userRepository, IQuestionRepository questionRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<RatingModel>> GetAllFromUser(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return _mapper.Map<IEnumerable<RatingModel>>(user.Ratings);
        }

        public async Task<RatingModel> GetQuestionRatingFromUser(Guid userId, Guid questionId)
        {
            var user = await _userRepository.GetUserById(userId);
            var rating = user.Ratings.FirstOrDefault(r => r.QuestionId == questionId);

            return _mapper.Map<RatingModel>(rating);
        }

        public async Task<IEnumerable<RatingModel>> GetAllFromQuestion(Guid questionId)
        {
            var question = await _questionRepository.GetById(questionId);

            var ratings = question.Ratings;
            return _mapper.Map<IEnumerable<RatingModel>>(ratings);
        }

        public IEnumerable<RatingModel> GetAllFromSurvey(Guid surveyId)
        {
            var questions = _questionRepository.GetAll().Where(q => q.SurveyId == surveyId);
            var ratings = new List<Rating>();

            foreach (var question in questions)
            {
                ratings.AddRange(question.Ratings);
            }

            return _mapper.Map<IEnumerable<RatingModel>>(ratings);
        }

        public async Task<RatingModel> GetRating(Guid questionId, Guid ratingId)
        {
            var question = await _questionRepository.GetById(questionId);
            var rating = question.Ratings.FirstOrDefault(r => r.Id == ratingId);

            return _mapper.Map<RatingModel>(rating);
        }

        public IEnumerable<RatingModel> GetUserRatingFromSurvey(Guid surveyId, Guid userId)
        {
            var ratingsFromSurvey = this.GetAllFromSurvey(surveyId);
            var ratings = new List<RatingModel>();

            ratings.AddRange(ratingsFromSurvey.Where(r => r.UserId == userId));

            return ratings;
        }

        public async Task Update(Guid questionId, Guid ratingId, UpdateRatingModel model)
        {
            var question = await _questionRepository.GetById(questionId);
            var rating = question.Ratings.FirstOrDefault(c => c.Id == ratingId);

            _mapper.Map(model, rating);
            _questionRepository.Update(question);

            await _questionRepository.SaveChanges();
        }
        public async Task<RatingModel> Add(Guid questionId, Guid userId, CreateRatingModel model)
        {
            var question = await _questionRepository.GetById(questionId);
            var user = await _userRepository.GetUserById(userId);

            var rating = _mapper.Map<Rating>(model);


            user.Ratings.Add(rating);
            _userRepository.Update(user);
            await _userRepository.SaveChanges();

            question.Ratings.Add(rating);
            _questionRepository.Update(question);
            await _questionRepository.SaveChanges();

            return _mapper.Map<RatingModel>(rating); 
        }

        public async Task Delete(Guid questionId, Guid ratingId)
        {
            var question = await _questionRepository.GetById(questionId);
            var rating = question.Ratings.FirstOrDefault(c => c.Id == ratingId);

            question.Ratings.Remove(rating);
            _questionRepository.Update(question);
            await _questionRepository.SaveChanges();
        }
    }
}
