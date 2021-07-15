using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence
{
    public sealed class TopicRepository: ITopicRepository
    {
        private readonly SurveysContext _context;

        public TopicRepository(SurveysContext context)
        {
            _context = context;
        }

        public async Task Create(Topic topic)
        {
            await _context.Topics.AddAsync(topic);
        }

        public void Update(Topic topic)
        {
            _context.Topics.Update(topic);
        }

        public void Delete(Topic topic)
        {
            _context.Remove(topic);
        }

        public IEnumerable<Topic> GetAll()
        {
            return _context.Topics;
        }

        public async Task<Topic> GetTopicById(Guid id)
        {
            return await _context.Topics
                .FirstAsync(i => i.Id == id);
            //return await _context.Surveys.FindAsync(id);
        }

        public async Task<Topic> GetTopicByTitle(string title)
        {
            return await _context.Topics
                .FirstAsync(i => i.Title == title);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
