using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly SurveysContext _context;
        public RolesController(SurveysContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public IEnumerable<IdentityRole> Get()
        {
            return _context.Roles.AsEnumerable();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post([FromBody] string roleName)
        {
            var roleToAdd = new IdentityRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            };

            _context.Roles.Add(roleToAdd);

            _context.SaveChanges();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var roleToDelete = _context.Roles.First(r => r.Id.Equals(id));

            _context.Roles.Remove(roleToDelete);

            _context.SaveChanges();
        }

    }
}
