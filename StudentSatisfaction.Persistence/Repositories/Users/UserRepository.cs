using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.Users
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly SurveysContext _context;


        public UserRepository(SurveysContext context)
        {
            _context = context;
        }

        public async Task Create(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users
                .FirstAsync(i => i.Id == id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
