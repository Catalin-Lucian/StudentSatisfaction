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
    internal abstract class UserMapping
    {
        internal static void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>()
                .Property(q => q.Id)
                .HasColumnName("Id")
                .IsRequired();

            modelBuilder.Entity<UserData>()
                .HasMany<SubmittedQuestion>(u => u.SubmittedQuestions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserData>()
                .HasMany<Rating>(u => u.Ratings)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserData>()
                .HasMany<Comment>(u => u.Comments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //exista intr-o singura parte!

            //modelBuilder.Entity<UserData>()
            //    .HasMany<Survey>(u => u.Surveys)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserData>()
                .HasMany<Notification>(u => u.Notifications)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
