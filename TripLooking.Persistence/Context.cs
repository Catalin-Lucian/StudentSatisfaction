using System;
using Microsoft.EntityFrameworkCore;

namespace StudentSatisfaction.Persistence
{
    public sealed class Context:DbContext
    {
        public Context(DbContextOptions<Context> options) : base()
        {
            Database.Migrate();
        }

        /*public DbSet<StudentSatisfaction> Trips { get; set; }*/
    }
}
