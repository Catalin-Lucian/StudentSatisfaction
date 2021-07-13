using StudentSatisfaction.Business.Surveys.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Users
{
    public interface IUsersService
    {
        public Task<IEnumerable<UserModel>> Get(Guid surveyId);
        public Task<UserModel> Add(Guid surveyId, CreateUserModel model);
        public Task Delete(Guid surveyId, Guid userId);
    }
}
