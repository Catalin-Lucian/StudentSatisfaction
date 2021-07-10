using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using System;

namespace StudentSatisfaction.Persistence
{
    public sealed class SurveysContext: DbContext
    {
        public SurveysContext(DbContextOptions<SurveysContext> options): base(options)
        {
            Database.Migrate();
        }

        public DbSet<Survey> Survey {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
