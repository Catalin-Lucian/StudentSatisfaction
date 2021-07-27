using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Persistence.Mappings;
using System;

namespace StudentSatisfaction.Persistence
{
    public sealed class SurveysContext: IdentityDbContext<ApplicationUser>
    {
        public SurveysContext(DbContextOptions<SurveysContext> options): base(options)
        {
            Database.Migrate();
        }

        public DbSet<Survey> Survey {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RatingMapping.Map(modelBuilder);
            QuestionMapping.Map(modelBuilder);
            SubmittedQuestionMapping.Map(modelBuilder);
            SurveyMapping.Map(modelBuilder);
            TopicMapping.Map(modelBuilder);
            CommentMapping.Map(modelBuilder);
            UserMapping.Map(modelBuilder);
            NotificationMapping.Map(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        //entities
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<SubmittedQuestion> SubmittedQuestions { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
