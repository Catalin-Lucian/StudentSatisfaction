using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Services;
using StudentSatisfaction.Persistence;
using System;
using System.Threading.Tasks;

//CreateSurveyModel --> genereaza in Mapper un ID nou
//SurveyModel ---> are ID-ul deja setat

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/survey")]
    [ApiController]
    public class SurveyController: ControllerBase
    {
        private readonly ISurveyService _surveyService;


        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var survey = await _surveyService.GetById(id);

            if(survey == null)
            {
                return BadRequest();
            }

            return Ok(survey);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSurveyModel model)
        {
            var result = await _surveyService.Create(model);

            return Created(result.Id.ToString(), null);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var trips = _surveyService.GetAll();

            return Ok(trips);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _surveyService.Delete(id);

            return NoContent();
        }

        //primeste ca parametru SurveyModel pt. ca nu vrem sa se genereze un nou PK pt. Survey-ul pe care vrem sa il updatam
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SurveyModel model)
        {
            await _surveyService.Update(id, model);

            return NoContent();
        }
    }
}
