using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/survey/{surveyId}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;


        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        //get all users that completed a certain survey
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid surveyId)
        {
            var users = await _usersService.Get(surveyId);

            return Ok(users);
        }

        //get a certain user that completed a certain survey
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get([FromRoute] Guid surveyId, [FromRoute] Guid userId)
        {
            var user = await _usersService.GetById(surveyId, userId);

            return Ok(user);
        }

        //// POST api/<UsersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<UsersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}


        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid surveyId, [FromRoute] Guid userId)
        {
            await _usersService.Delete(surveyId, userId);

            return NoContent();
        }
    }
}
