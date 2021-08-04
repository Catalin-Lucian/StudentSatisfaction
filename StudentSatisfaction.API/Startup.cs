using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StudentSatisfaction.Persistence;
using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Business.Surveys.Services;
using StudentSatisfaction.Business.Surveys.Services.Comments;
using StudentSatisfaction.Business.Surveys.Services.Questions;
using StudentSatisfaction.Business.Surveys;
using StudentSatisfaction.Business.Surveys.Services.Topics;
using StudentSatisfaction.Business.Surveys.Services.Users;
using StudentSatisfaction.Persistence.Repositories.Users;
using StudentSatisfaction.Business.Surveys.Services.Notifications;
using StudentSatisfaction.Persistence.Repositories.Comments;
using StudentSatisfaction.Business.Surveys.Services.SubmittedQuestions;
using StudentSatisfaction.Persistence.Repositories.SubmittedQuestions;
using StudentSatisfaction.Business.Surveys.Services.Ratings;
using StudentSatisfaction.Persistence.Repositories.Questions;
using StudentSatisfaction.Persistence.Repositories.Ratings;
using StudentSatisfaction.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Models.Ratings;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Business.Validators;

namespace StudentSatisfaction.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            services.AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddDbContext<SurveysContext>(options => options.UseSqlServer(Configuration.GetConnectionString("StudentSatisfactionConnection")));


            // For Identity  
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SurveysContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.AddAutoMapper(config =>
            {
                config.AddProfile<SurveyMappingProfile>();
            });

            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "StudentSatisfaction",
                    Description = "Authentication and Authorization in ASP.NET 5 with JWT and Swagger"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            services
                .AddScoped<ISurveyRepository, SurveyRepository>()
                .AddScoped<ITopicRepository, TopicRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<ISubmittedQuestionRepository, SubmittedQuestionRepository>()
                .AddScoped<IQuestionRepository, QuestionRepository>()
                .AddScoped<IRatingRepository, RatingRepository>()
                .AddScoped<ISurveyService, SurveyService>()
                .AddScoped<IQuestionService, QuestionService>()
                .AddScoped<ITopicsService, TopicsService>()
                .AddScoped<IUsersService, UserService>()
                .AddScoped<ICommentsService, CommentsService>()
                .AddScoped<ISubmittedQuestionsService, SubmittedQuestionService>()
                .AddScoped<IRatingService, RatingService>()
                .AddScoped<INotificationsService, NotificationsService>();

            services.AddScoped<IValidator<CreateRatingModel>, CreateRatingModelValidator>();
            services.AddScoped<IValidator<UpdateRatingModel>, UpdateRatingModelValidator>();
            services.AddScoped<IValidator<CreateUserModel>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdateUserModel>, UpdateUserModelValidator>();


            services
                .AddMvc()
                .AddFluentValidation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET 5 Web API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<SurveysContext>();
            dbContext.Database.EnsureCreated();

            //Checks if role "User" exists if not creates it
            var userRole = dbContext.Roles.FirstOrDefault(r => r.Name == "User");
            if (userRole == null)
            {
                var roleToAdd = new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                };

                dbContext.Roles.Add(roleToAdd);
                dbContext.SaveChanges();

            }

            //Checks if role "Admin" exists if not creates it
            var adminRole = dbContext.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (adminRole == null)
            {
                var roleToAdd = new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };

                dbContext.Roles.Add(roleToAdd);
                dbContext.SaveChanges();

            }
        }
    }
}
