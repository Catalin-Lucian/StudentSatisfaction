using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Persistence;
using System;
using System.Threading.Tasks;

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/v1/survey")]
    [ApiController]
    public class SurveyController: ControllerBase
    {
        private readonly ISurveyRepository _surveyRepository;
        public SurveyController(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            var result = await _surveyRepository.GetSurveyById(id);
            return Ok(result);
        }
    }
}
