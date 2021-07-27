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
        IEnumerable<UserData> GetAll();
        Task<UserData> GetUserById(Guid id);
        Task Create(UserData userData);
        void Update(UserData userData);
        void Delete(UserData userData);
        Task DeleteCredentials(Guid userId);
        Task SaveChanges();
    }
}
