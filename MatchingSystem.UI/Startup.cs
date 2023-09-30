using System;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature._Repository;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Matching.service;
using MatchingSystem.DataLayer.Feature.Role.service;
using MatchingSystem.DataLayer.Feature.Stage.service;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.User.service;
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
using MatchingSystem.Service.Tutor;
using MatchingSystem.UI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
                .AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                .AddRazorRuntimeCompilation();

            var optionsBuilder = new DbContextOptionsBuilder<DiplomaMatchingContext>();
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseLazyLoadingProxies();

            var dbContext = new DiplomaMatchingContext(optionsBuilder.Options);
            services.AddSingleton<DbContext, DiplomaMatchingContext>(_ => dbContext);
            services
                .AddSingleton<IMatchingRepository, MatchingRepository>();

            services.AddTransient<IMatchingRepositoryDapper, MatchingRepositoryDapper>(_ =>
                new MatchingRepositoryDapper(connectionString));
            
            services.AddTransient<IStudentRepositoryOld, StudentRepositoryOld>(_ =>
                new StudentRepositoryOld(connectionString));

            services.AddTransient<ITutorRepository, TutorRepository>(_ =>
                new TutorRepository(connectionString));

            services.AddTransient<IProjectRepository, ProjectRepository>(_ =>
                new ProjectRepository(connectionString));
            services.AddTransient<IGroupRepository, GroupRepository>(_ =>
                new GroupRepository(connectionString));
            services.AddTransient<IUserRepositoryOld, UserRepositoryOld>(_ =>
                new UserRepositoryOld(connectionString));

           // services.AddTransient<IMatchingRepository, MatchingRepository>(options =>
           //     new MatchingRepository());
        

            services.AddTransient<IExecutiveRepository, ExecutiveRepository>(_ =>
                new ExecutiveRepository(connectionString));
            services.AddSingleton<IStatisticsRepository, StatisticsRepository>(_ =>
                new StatisticsRepository(connectionString));
            services.AddSingleton<IMatchingRepository, MatchingRepository>(_ =>
                new MatchingRepository(dbContext));

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
            services.AddSingleton<IUserRepository, UserRepository>();            
            services.AddSingleton<IRoleRepository, RoleRepository>();
            services.AddSingleton<ITutorsParsingService, TutorParsingService>();            
            services.AddSingleton<IStudentsParsingService, StudentsParsingService>();            
            services.AddSingleton<IDocumentsProcessingService, DocumentsProcessingService>();            
            services.AddSingleton<IMatchingInitializationService, MatchingInitializationService>();            
            services.AddSingleton<IMonitoringService, MonitoringService>();
            services.AddSingleton<IStageTransitionService, StageTransitionService>();
            services.AddSingleton<IMatchingService, MatchingService>();
            services.AddSingleton<ICommonQuotaRepository, CommonQuotaRepository>();
            services.AddSingleton<IStageRepository, StageRepository>();
            services.AddSingleton<IRoleRepository, RoleRepository>();
            services.AddSingleton<IStudentRepository, StudentRepository>();
            services.AddSingleton<IDictionaryRepository, DictionaryRepository>();
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