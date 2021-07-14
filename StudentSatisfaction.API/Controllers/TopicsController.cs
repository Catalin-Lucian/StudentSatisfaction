using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Services.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/survey/{surveyId}/topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicsService _topicsService;


        public TopicsController(ITopicsService topicsService)
        {
            _topicsService = topicsService;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid surveyId)
        {
            var topics = await _topicsService.Get(surveyId);

            return Ok(topics);
        }

        // GET api/<TopicsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TopicsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TopicsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TopicsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
