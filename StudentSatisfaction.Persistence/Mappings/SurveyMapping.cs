using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Mappings
{
    internal abstract class SurveyMapping
    {
        internal static void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Survey>()
                .Property(s => s.Id)
                .HasColumnName("id")
                .IsRequired();

            modelBuilder.Entity<Survey>()
                .HasMany<Question>(s => s.Questions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Survey>()
                .HasMany<SubmittedQuestion>(s => s.SubmittedQuestions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Survey>()
                .HasMany<Comment>(s => s.Comments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //UserSurvey  ----> points!
            modelBuilder.Entity<Survey>()
                .HasMany<User>(s => s.Users)
                .WithMany(s => s.Surveys)
                .UsingEntity(j => j.ToTable("UserSurveys"));

            modelBuilder.Entity<Survey>()
                .HasMany<Topic>(s => s.Topics)
                .WithMany(s => s.Surveys)
                .UsingEntity(j => j.ToTable("SurveysTopics"));
        }
    }
}
