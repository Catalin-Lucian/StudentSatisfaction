using System;
using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;

namespace StudentSatisfaction.Persistence
{
    public sealed class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Survey> Surveys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //One Question has many Ratings
            modelBuilder.Entity<Question>()
                .HasMany<Rating>(q => q.Ratings)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //define Question
            modelBuilder.Entity<Question>()
                .Property(q => q.Id)
                .IsRequired()
                .ValueGeneratedNever();  //check

            //define Rating
            modelBuilder.Entity<Rating>()
                .Property(r => r.Id)
                .IsRequired()
                .ValueGeneratedNever(); ///check

            //define Comment
            modelBuilder.Entity<Comment>()
                .Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedNever(); //check

            //define SubmittedQuestions
            modelBuilder.Entity<SubmittedQuestion>()
                .Property(sq => sq.Id)
                .IsRequired()
                .ValueGeneratedNever();  //check

            //define User
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .IsRequired()
                .ValueGeneratedNever();  //check

            //define UserSurvey
            modelBuilder.Entity<UserSurvey>()
                .Property(us => us.Id)
                .IsRequired()
                .ValueGeneratedNever();

            //One Survey has many Comments 
            modelBuilder.Entity<Survey>()
                .HasMany<Comment>(c => c.Comments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One Survey has many Questions
            modelBuilder.Entity<Survey>()
                .HasMany<Question>(q => q.Questions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One Survey has many SubmittedQuestions
            modelBuilder.Entity<Survey>()
                .HasMany<SubmittedQuestion>(sq => sq.SubmittedQuestions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //***********************************************************//
            //One Survey has many Users (UserSurvey)
            modelBuilder.Entity<UserSurvey>()
                .HasMany<User>(u => u.Users)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One User completed many Surveys (UserSurvey)
            modelBuilder.Entity<UserSurvey>()
                .HasMany<Survey>(s => s.Surveys)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            //***********************************************************//

            //define Survey
            modelBuilder.Entity<Survey>()
                .Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedNever(); //check

        }
    }
}
