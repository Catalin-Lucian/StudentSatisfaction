using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models.Comments;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Comments
{
    public sealed class CommentsService : ICommentsService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IMapper _mapper;


        public CommentsService(ISurveyRepository surveyRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentModel>> Get(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<IEnumerable<CommentModel>>(survey.Comments);
        }

        public async Task<CommentModel> Add(Guid surveyId, CreateCommentModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var comment = _mapper.Map<Comment>(model);

            survey.Comments.Add(comment);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();

            return _mapper.Map<CommentModel>(comment);
        }

        public async Task Delete(Guid surveyId, Guid commentId)
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
    }
}
