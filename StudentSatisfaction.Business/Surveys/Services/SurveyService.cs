using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services
{
    public sealed class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IMapper _mapper;

        public SurveyService(ISurveyRepository surveyRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
        }

        public IEnumerable<SurveyModel> GetAll()
        {
            var surveys = _surveyRepository.GetAll();

            return _mapper.Map<IEnumerable<SurveyModel>>(surveys);
        }

        public async Task<SurveyModel> GetById(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<SurveyModel>(survey);
        }

        public async Task<SurveyModel> Create(CreateSurveyModel model)
        {
            var survey = _mapper.Map<Survey>(model);
            await _surveyRepository.Create(survey);
            await _surveyRepository.SaveChanges();

            return _mapper.Map<SurveyModel>(survey);
        }

        public async Task Delete(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            _surveyRepository.Delete(survey);
            await _surveyRepository.SaveChanges();
        }

        public async Task Update(Guid surveyId, UpdateSurveyModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            _mapper.Map(model, survey);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();
        }
    }
}
