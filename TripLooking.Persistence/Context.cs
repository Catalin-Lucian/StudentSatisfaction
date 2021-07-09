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


            //define Survey
            modelBuilder.Entity<Survey>()
                .Property(s => s.Id)
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

            //define Topic
            modelBuilder.Entity<Topic>()
                .Property(t => t.Id)
                .IsRequired()
                .ValueGeneratedNever();

            //define FacultyDetails
            modelBuilder.Entity<FacultyDetails>()
                .Property(fd => fd.Id)
                .IsRequired()
                .ValueGeneratedNever();

            //define PersonalDetails
            modelBuilder.Entity<PersonalDetails>()
                .Property(pd => pd.Id)
                .IsRequired()
                .ValueGeneratedNever();

            //define Login
            modelBuilder.Entity<Login>()
                .Property(l => l.Id)
                .IsRequired()
                .ValueGeneratedNever();

            //define SurveysTopics
            modelBuilder.Entity<SurveysTopics>()
                .Property(st => st.Id)
                .IsRequired()
                .ValueGeneratedNever();


            //RELATIONS - Questions

            //One Question has many Ratings
            modelBuilder.Entity<Question>()
                .HasMany<Rating>(q => q.Ratings)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);


            //RELATIONS - Survey

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

            //One Survey has many Comments 
            modelBuilder.Entity<Survey>()
                .HasMany<Comment>(c => c.Comments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One Survey has many Topics (SurveysTopics)
            modelBuilder.Entity<Survey>()
                .HasMany<SurveysTopics>(st => st.SurveysTopics)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One Survey has many Users (UserSurveys)
            modelBuilder.Entity<Survey>()
                .HasMany<UserSurvey>(us => us.UserSurveys)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);


            //RELATIONS - User
            //One User can have many SubmittedQuestions 
            modelBuilder.Entity<User>()
                .HasMany<SubmittedQuestion>(sq => sq.SubmittedQuestions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One User can have many Ratings 
            modelBuilder.Entity<User>()
                .HasMany<Rating>(r => r.Ratings)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One User can have many Comments
            modelBuilder.Entity<User>()
                .HasMany<Comment>(c => c.Comments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One User can respond to many Surveys (UserSurveys)
            modelBuilder.Entity<User>()
                .HasMany<UserSurvey>(us => us.UserSurveys)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One User can have only one set of Login 
            modelBuilder.Entity<User>()
                .HasOne<Login>()   //?????
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //One User can have only one set of PersonalDetails
            modelBuilder.Entity<User>()
                .HasOne<PersonalDetails>()   //?????
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);


            //RELATIONS - PersonalDetails
            //One set of PersonalDetails can have only one set of FacultyDetails
            modelBuilder.Entity<PersonalDetails>()
                .HasOne<FacultyDetails>()   //?????
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);


            //RELATIONS - Topic
            //One Topic can corespond to many Surveys (SurveysTopics)
            modelBuilder.Entity<Topic>()
                .HasMany<SurveysTopics>(st => st.SurveysTopics)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
