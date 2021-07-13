using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Services;
using StudentSatisfaction.Persistence;
using System;
using System.Threading.Tasks;

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/survey")]
    [ApiController]
    public class SurveyController: ControllerBase
    {
        //private readonly ISurveyRepository _surveyRepository;
        //public SurveyController(ISurveyRepository surveyRepository)
        //{
        //    _surveyRepository = surveyRepository;
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get([FromRoute]Guid id)
        //{
        //    var result = await _surveyRepository.GetSurveyById(id);
        //    return Ok(result);
        //}

        private readonly ISurveyService _surveyService;
        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await _surveyService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSurveyModel model)
        {
            var result = await _surveyService.Create(model);

            return Created(result.Id.ToString(), null);
        }
    }
}
