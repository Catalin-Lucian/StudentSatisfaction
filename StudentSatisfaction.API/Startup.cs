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

            services.AddDbContext<SurveysContext>(config =>
            {
                config.UseSqlServer(Configuration.GetConnectionString("StudentSatisfactionConnection"));
            });

            //services.AddControllers();
            services.AddSwaggerGen();

            services
                .AddScoped<ISurveyRepository, SurveyRepository>();
            //    .AddScoped<ISurveyService, SurveyService>()
            //    .AddScoped<ICommentsService, CommentsService>()
            //    .AddScoped<IQuestionService, QuestionService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentSatisfaction.API v1"));
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API - v1");
            });

            app.UseHttpsRedirection()
               .UseRouting()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
               {
                    endpoints.MapControllers();
               });

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<SurveysContext>();
            dbContext.Database.EnsureCreated();
        }
    }
}
