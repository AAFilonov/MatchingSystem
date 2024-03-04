using System;
using MatchingSystem.Constants;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.Allocation.repo;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;
using MatchingSystem.DataLayer.Feature.Dictionary.repo;
using MatchingSystem.DataLayer.Feature.Group.repo;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Role.repo;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.DataLayer.Feature.Statistics.repo;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.StudentPreference.repo;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.Feature.TutorChoice.repo;
using MatchingSystem.DataLayer.Feature.User.repo;
using MatchingSystem.Service.Allocation;
using MatchingSystem.Service.DocumentsProcessing;
using MatchingSystem.Service.Executive;
using MatchingSystem.Service.Matching;
using MatchingSystem.Service.MatchingInitialization;
using MatchingSystem.Service.Monitoring;
using MatchingSystem.Service.Notification;
using MatchingSystem.Service.Projects;
using MatchingSystem.Service.Quotas;
using MatchingSystem.Service.Stage;
using MatchingSystem.Service.StageTransition;
using MatchingSystem.Service.Statistics;
using MatchingSystem.Service.Student;
using MatchingSystem.Service.Tutor;
using MatchingSystem.UI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
            SystemParameters.Init(Configuration);
            services.AddDistributedMemoryCache();
            services.AddMvc();
            services
                .AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                .AddRazorRuntimeCompilation();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new PathString("/Home/Login"));
            
            var optionsBuilder = new DbContextOptionsBuilder<DiplomaMatchingContext>();
            optionsBuilder.UseSqlServer(SystemParameters.getInstance().CONNECTION_STRING);
            optionsBuilder.UseLazyLoadingProxies();
            
            services.AddTransient<IMatchingRepositoryDapper, MatchingRepositoryDapper>(_ =>
                new MatchingRepositoryDapper(SystemParameters.getInstance().CONNECTION_STRING));
            services.AddTransient<IStudentRepositoryOld, StudentRepositoryOld>(_ =>
                new StudentRepositoryOld(SystemParameters.getInstance().CONNECTION_STRING));
            services.AddTransient<IProjectRepositoryOld, ProjectRepositoryOldDapper>(_ =>
                new ProjectRepositoryOldDapper(SystemParameters.getInstance().CONNECTION_STRING));
            services.AddTransient<IExecutiveServiceOld, ExecutiveServiceDapper>(_ =>
                new ExecutiveServiceDapper(SystemParameters.getInstance().CONNECTION_STRING));
            services.AddSingleton<IStatisticsRepositoryOld, StatisticsRepositoryOldDapper>(_ =>
                new StatisticsRepositoryOldDapper(SystemParameters.getInstance().CONNECTION_STRING));

            //--Repository
            services.AddSingleton<IMatchingRepository, MatchingRepository>();
            services.AddSingleton<IQuotaNotificationService, QuotaNotificationService>();
            services.AddSingleton<IStatisticsRepository, StatisticsRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();            
            services.AddSingleton<IRoleRepository, RoleRepository>();
            services.AddSingleton<ICommonQuotaRepository, CommonQuotaRepository>();
            services.AddSingleton<IQuotaNotificationRepository, QuotaNotificationRepository>();
            services.AddSingleton<IQuotaRequestRepository, QuotaRequestRepository>();
            services.AddSingleton<IStageRepository, StageRepository>();
            services.AddSingleton<IRoleRepository, RoleRepository>();
            services.AddSingleton<IStudentRepository, StudentRepository>();
            services.AddSingleton<IDictionaryRepository, DictionaryRepository>();
            services.AddSingleton<IGroupRepository, GroupRepository>();
            services.AddSingleton<ITutorRepository, TutorRepository>();
            services.AddSingleton<IAllocationRepository, AllocationRepository>();
            services.AddSingleton<IProjectRepository,ProjectRepository>();
            services.AddSingleton<IStudentPreferenceRepository,StudentPreferenceRepository>();
            services.AddSingleton<ITutorsChoiceRepository,TutorsChoiceRepository>();
            
            //--Services
            services.AddSingleton<IStageService, StageService>();
            services.AddSingleton<IMatchingService, MatchingService>();
            services.AddSingleton<ITutorsParsingService, TutorParsingService>();            
            services.AddSingleton<IStudentsParsingService, StudentsParsingService>();            
            services.AddSingleton<IDocumentsProcessingService, DocumentsProcessingService>();
            services.AddSingleton<IMatchingInitializationService, MatchingInitializationService>();            
            services.AddSingleton<IMonitoringService, MonitoringService>();
            services.AddSingleton<IStageTransitionService, StageTransitionService>();
            services.AddSingleton<IAllocationService, AllocationService>();
            services.AddSingleton<IExecutiveService, ExecutiveService>();
            services.AddSingleton<IProjectsService, ProjectsService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<ICommonQuotaService, CommonCommonQuotaService>();
            services.AddSingleton<ITutorService, TutorService>();
            services.AddSingleton<IStatisticsService, StatisticsService>();
            services.AddSingleton<IMainStatisticsService, MainStatisticsService>();
            
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.Name = "SessionStorage";
            });

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