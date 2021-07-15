using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence
{
    public interface ITopicRepository
    {
        Task Create(Topic topic);
        void Update(Topic topic);
        void Delete(Topic topic);
        IEnumerable<Topic> GetAll();
        Task<Topic> GetTopicById(Guid id);
        Task<Topic> GetTopicByTitle(string title);
        Task SaveChanges();
    }
}
