using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Business.Surveys.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;


        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _usersService.GetAllUsers();

            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid userId)
        {
            var user = await _usersService.GetUserById(userId);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserModel model)
        {
            var user = await _usersService.Create(model);

            return Created(user.Id.ToString(), null);
        }


        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(Guid userId, [FromBody] UpdateUserModel model)
        {
            await _usersService.Update(userId, model);

            return NoContent();
        }


        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid userId)
        {
            await _usersService.Delete(userId);

            return NoContent();
        }
    }
}
