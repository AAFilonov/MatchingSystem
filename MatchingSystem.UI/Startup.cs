using System;
using MatchingSystem.Data;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Repository;
using MatchingSystem.Service;
using MatchingSystem.Service.Allocation;
using MatchingSystem.Service.DocumentsProcessing;
using MatchingSystem.Service.Executive;
using MatchingSystem.Service.Follow;
using MatchingSystem.Service.MatchingInitialization;
using MatchingSystem.Service.Monitoring;
using MatchingSystem.Service.Notification;
using MatchingSystem.Service.Projects;
using MatchingSystem.Service.Quotas;
using MatchingSystem.Service.Statistics;
using MatchingSystem.Service.Student;
using MatchingSystem.Service.Tutor;
using MatchingSystem.Service.User;
using MatchingSystem.UI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MatchingSystem.UI
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDistributedMemoryCache();
            services.AddMvc();
            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            var optionsBuilder = new DbContextOptionsBuilder<DiplomaMatchingContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var dbContext = new DiplomaMatchingContext(optionsBuilder.Options);
            services.AddSingleton<DbContext, DiplomaMatchingContext>(options => dbContext);
            services.AddSingleton<Data.Feature.User.IUserRepository, Data.Feature.User.UserRepository>();
            services
                .AddSingleton<Data.Feature.Matching.IMatchingRepository, Data.Feature.Matching.MatchingRepository>();


            
            services.AddTransient<IStudentRepository, StudentRepository>(options =>
                new StudentRepository(connectionString));

            services.AddTransient<IDictionaryRepository, DictionaryRepository>(options =>
                new DictionaryRepository(connectionString));

            services.AddTransient<ITutorRepository, TutorRepository>(options =>
                new TutorRepository(connectionString));

            services.AddTransient<IProjectRepository, ProjectRepository>(options =>
                new ProjectRepository(connectionString));
            services.AddTransient<IGroupRepository, GroupRepository>(options =>
                new GroupRepository(connectionString));
            services.AddTransient<IUserRepository, UserRepository>(options =>
                new UserRepository(connectionString));

            services.AddTransient<IMatchingRepository, MatchingRepository>(options =>
                new MatchingRepository(connectionString));

            services.AddTransient<IExecutiveRepository, ExecutiveRepository>(options =>
                new ExecutiveRepository(connectionString));
            services.AddSingleton<IStatisticsRepository, StatisticsRepository>(options =>
                new StatisticsRepository(connectionString));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login"));
            
            
      

            //--Services
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IStatisticsService, StatisticsService>();
            services.AddSingleton<IAllocationService, AllocationService>();
            services.AddSingleton<IExecutiveService, ExecutiveService>();
            services.AddSingleton<IProjectsService, ProjectsService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<IQuotasService, QuotasService>();
            services.AddSingleton<ITutorService, TutorService>();
            services.AddSingleton<IUserService, UserService>();            
            services.AddSingleton<ITutorsParsingService, TutorParsingService>();            
            services.AddSingleton<IStudentsParsingService, StudentsParsingService>();            
            services.AddSingleton<IDocumentsProcessingService, DocumentsProcessingService>();            
            services.AddSingleton<IMatchingInitializationService, MatchingInitializationService>();            
            services.AddSingleton<IMonitoringService, MonitoringService>();            
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IStageTransitionService, StageTransitionService>();
            //--Services



            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.Name = "SessionStorage";
            });

            var useMigrations = (bool) Configuration.GetValue(typeof(bool), "ApplicationSettings:UseMigrations");
           
        }

        public void Configure(IApplicationBuilder app)
        {
            var env = Configuration.GetValue(typeof(string), "ApplicationSettings:Environment").ToString();

            if (env == "Development")
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCors();
            app.UseSession();
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseRouting();
            app.UseAuthorization();
            app.UseRequestLogger(Configuration.GetConnectionString("DefaultConnection"));
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}"
                );
            });
        }
    }
}