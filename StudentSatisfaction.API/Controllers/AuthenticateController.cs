using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentSatisfaction.Business.Surveys.Models.Authentication;
using StudentSatisfaction.Business.Surveys.Models.RegisterModel;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Business.Surveys.Services.Users;
using StudentSatisfaction.Entities;
using StudentSatisfaction.Persistence;

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SurveysContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUsersService _usersService;

        public AuthenticateController(UserManager<ApplicationUser> userManager,
                   SurveysContext context,
                   IConfiguration configuration, IUsersService usersService
)
        {
            this.userManager = userManager;
            _context = context;
            _configuration = configuration;
            _usersService = usersService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //var user = await userManager.FindByNameAsync(model.Username);
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("userId",user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Username already exists!" });

            ////////
            userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Email already exists!" });
            ////////

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            //adaugare rol de "UserData" pt. noul user creat
            var createdUser = await userManager.FindByNameAsync(model.Username);
            await userManager.AddToRoleAsync(user, "User");

            //Adaug noul UserData creat la Register si in tabela UsersData
            var newUser = new UserModel()
            {
                Id = new Guid(user.Id),
                Type = "User",
                Password = user.PasswordHash,
                Name = model.Name,
                Username = model.Username,
                Email = model.Email,
                BirthDate = model.BirthDate,
                FacultyName = model.FacultyName
            };


            await _usersService.Create(newUser);
            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("assignRole")]
        public async Task<IActionResult> AssignRole(string username, string roleId)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User not found!" });

            string roleName = _context.Roles.First(r => r.Id.Equals(roleId)).Name;

            await userManager.AddToRoleAsync(user, roleName);


            //odata asignat un rol pt. un utilizator, se face update si la campul type din tabela UsersData
            var userToUpdate = await _usersService.GetUserById(new Guid(user.Id));
            var userUpdatedModel = new UpdateUserModel()
            {
                Type = roleName,
                Password = userToUpdate.Password,
                Name = userToUpdate.Name,
                Username = userToUpdate.Username,
                Email = userToUpdate.Email,
                BirthDate = userToUpdate.BirthDate,
                FacultyName = userToUpdate.FacultyName
            };
            await _usersService.Update(new Guid(user.Id), userUpdatedModel);

            return Ok(new { Status = "Success", Message = "User role successfully updated!" });
        }
    }
}
