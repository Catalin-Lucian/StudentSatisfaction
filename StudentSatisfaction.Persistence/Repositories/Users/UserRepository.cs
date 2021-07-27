using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSatisfaction.Entities;

namespace StudentSatisfaction.Persistence.Repositories.Users
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly SurveysContext _context;


        public UserRepository(SurveysContext context)
        {
            _context = context;
        }

        public async Task Create(UserData userData)
        {
            await _context.UsersData.AddAsync(userData);
        }

        public void Update(UserData userData)
        {
            _context.UsersData.Update(userData);
        }

        public void Delete(UserData userData)
        {
            //remove the user from the Users table
            _context.UsersData.Remove(userData);
        }

        public async Task DeleteCredentials(Guid userId)
        {
            //remove the user from the AspNetUsers table
            var user = await _context.Users.FirstAsync(u => u.Id == userId.ToString());
            _context.Users.Remove(user);
        }

        public IEnumerable<UserData> GetAll()
        {
            return _context.UsersData;
        }

        public async Task<UserData> GetUserById(Guid id)
        {
            return await _context.UsersData
                .Include(x => x.Notifications)
                .Include(x => x.Surveys)
                .Include(x => x.Comments)
                .Include(x => x.Ratings)
                .Include(x => x.SubmittedQuestions)
                .FirstAsync(i => i.Id == id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
