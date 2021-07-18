using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.Ratings
{
    public interface IRatingRepository
    {
        IEnumerable<Rating> GetAll();
        Task<Rating> GetById(Guid id);
        Task Create(Rating rating);
        void Update(Rating rating);
        void Delete(Rating rating);
        Task SaveChanges();
    }
}
