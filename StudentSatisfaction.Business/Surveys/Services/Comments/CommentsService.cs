using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models.Comments;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using StudentSatisfaction.Persistence.Repositories.Comments;
using StudentSatisfaction.Persistence.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Comments
{
    public sealed class CommentsService : ICommentsService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public CommentsService(ISurveyRepository surveyRepository, IUserRepository userRepository, ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _surveyRepository = surveyRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CommentModel> GetCommentById(Guid commentId)
        {
            var comment = await _commentRepository.GetCommentById(commentId);

            return _mapper.Map<CommentModel>(comment);
        }

        public async Task<IEnumerable<CommentModel>> GetCommentsFromSurvey(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<IEnumerable<CommentModel>>(survey.Comments);
        }

        public async Task<IEnumerable<CommentModel>> GetCommentsFromUser(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);

            return _mapper.Map<IEnumerable<CommentModel>>(user.Comments);
        }

        public async Task<CommentModel> Add(Guid surveyId, Guid userId, CreateCommentModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var user = await _userRepository.GetUserById(userId);
            var comment = _mapper.Map<Comment>(model);


            //creez comentariul in Tabela Comments
            await _commentRepository.Create(comment);
            await _commentRepository.SaveChanges();

            survey.Comments.Add(comment);
            user.Comments.Add(comment);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();

            _userRepository.Update(user);
            await _userRepository.SaveChanges();

            return _mapper.Map<CommentModel>(comment);
        }

        public async Task Update(Guid commentId, UpdateCommentModel model)
        {
            var comment = await _commentRepository.GetCommentById(commentId);
            _mapper.Map(model, comment);

            _commentRepository.Update(comment);
            await _commentRepository.SaveChanges();
        }

        public async Task DeleteCommentFromSurvey(Guid surveyId, Guid commentId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var commentToRemove = survey.Comments.FirstOrDefault(c => c.Id == commentId);

            if(commentToRemove != null)
            {
                survey.Comments.Remove(commentToRemove);
            }

            _surveyRepository.Update(survey);

            await _surveyRepository.SaveChanges();
        }

        public async Task DeleteCommentFromUser(Guid userId, Guid commentId)
        {
            var user = await _userRepository.GetUserById(userId);
            var commentToRemove = user.Comments.FirstOrDefault(c => c.Id == commentId);

            if (commentToRemove != null)
            {
                user.Comments.Remove(commentToRemove);
            }

            _userRepository.Update(user);

            await _userRepository.SaveChanges();
        }
    }
}
