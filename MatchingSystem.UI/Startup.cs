using System;
using MatchingSystem.Data;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Repository;
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

            services.AddTransient<IStudentRepository, StudentRepository>(options =>
                new StudentRepository(connectionString));

            services.AddTransient<IDictionaryRepository, DictionaryRepository>(options =>
                new DictionaryRepository(connectionString));

            services.AddTransient<ITutorRepository, TutorRepository>(options =>
                new TutorRepository(connectionString));

            services.AddTransient<IProjectRepository, ProjectRepository>(options =>
                new ProjectRepository(connectionString));

            services.AddTransient<IUserRepository, UserRepository>(options =>
                new UserRepository(connectionString));

            services.AddTransient<IMatchingRepository, MatchingRepository>(options =>
                new MatchingRepository(connectionString));

            services.AddTransient<IExecutiveRepository, ExecutiveRepository>(options =>
                new ExecutiveRepository(connectionString));
            services.AddTransient<IStatisticsRepository, StatisticsRepository>(options =>
                new StatisticsRepository(connectionString));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login"));
            services.AddSingleton<DbContext, DiplomaMatchingContext>(options => new DiplomaMatchingContext(connectionString));
            services.AddSingleton<MatchingSystem.Data.Feature.User.IUserRepository, MatchingSystem.Data.Feature.User.UserRepository>();
            services.AddSingleton<MatchingSystem.Data.Feature.Matching.IMatchingRepository, MatchingSystem.Data.Feature.Matching.MatchingRepository>();
            
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