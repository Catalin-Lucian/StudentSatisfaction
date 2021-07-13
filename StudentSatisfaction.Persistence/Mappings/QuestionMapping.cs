using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Mappings
{
    internal abstract class QuestionMapping
    {
        internal static void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .Property(q => q.Id)
                .HasColumnName("Id")
                .IsRequired();

            modelBuilder.Entity<Question>()
                .Property(q => q.QuestionText)
                .HasColumnName("QuestionText")
                .IsRequired();

            modelBuilder.Entity<Question>()
                .Property(q => q.Type)
                .HasColumnName("Type")
                .IsRequired();

            modelBuilder.Entity<Question>()
                .Property(q => q.SurveyId)
                .HasColumnName("SurveyId")
                .IsRequired();

            modelBuilder.Entity<Question>()
                .HasMany<Rating>(q => q.Ratings)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
