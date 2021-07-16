using StudentSatisfaction.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.Users
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        Task<User> GetUserById(Guid id);
        Task Create(User user);
        void Update(User user);
        void Delete(User user);
        Task SaveChanges();
    }
}
