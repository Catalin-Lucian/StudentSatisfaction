using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.Ratings
{
    public sealed class RatingRepository : IRatingRepository
    {
        private readonly SurveysContext _context;

        public RatingRepository(SurveysContext context)
        {
            _context = context;
        }

        public async Task Create(Rating rating)
        {
           await _context.Ratings.AddAsync(rating);
        }

        public void Delete(Rating rating)
        {
            _context.Ratings.Remove(rating);
        }

        public IEnumerable<Rating> GetAll()
        {
            return _context.Ratings;
        }

        public Task<Rating> GetById(Guid id)
        {
            return _context.Ratings.FirstAsync(c => c.Id == id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Rating rating)
        {
            _context.Ratings.Update(rating);
        }
    }
}
