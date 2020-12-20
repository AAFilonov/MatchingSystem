using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using MatchingSystem.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Scrypt;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using MatchingSystem.UI.ViewModels;

namespace MatchingSystem.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext context;

        public HomeController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            SessionData data = new SessionData();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //TODO GetUserAsync
                var usr = context.Users
                    .FromSqlRaw("select * from napp.get_User(null, @Login)",
                        new SqlParameter("@Login", HttpContext.User.Identity.Name)).FirstOrDefault();

                //TODO GetAllRolesAsync
                data.RolesMatchings = await context.RolesAndMatchings.FromSqlRaw(
                    "select * from napp.get_UserRolesMatchings(@UserID, null)",
                    new SqlParameter("@UserID", usr!.UserID)).ToListAsync();
                data.User = usr;
                data.CountRoles = data.RolesMatchings.Count;

                var selectedMatching = HttpContext.Request.Cookies["selectedMatching"];
                data.SelectedMatching = Convert.ToInt32(selectedMatching);

                string selectedRole = HttpContext.Request.Cookies["selectedRole"];
                data.SelectedRole = selectedRole;

                data.CurrentStage = await context.GetCurrentStageAsync(data.SelectedMatching);

                await context.SetLastVisitDate(usr.UserID, selectedRole, System.Convert.ToInt32(selectedMatching));

                HttpContext.Session.Set<SessionData>("Data", data);

                return RedirectToLk(selectedRole);
            }

            AuthViewModel user = new AuthViewModel();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel auth)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            SessionData data = new SessionData();

            SqlParameter login = new SqlParameter("@Login", auth.Login);
            //TODO GetUserIdByLoginAsync
            int userID = context.GetInt.FromSqlRaw("select napp.get_UserID(@Login) as Value", login).FirstOrDefault().Value;

            if (userID == -1)
            {
                ModelState.AddModelError("login", "Неверный логин");
                return View();
            }
            
            SqlParameter uid = new SqlParameter("@UserID", userID);
            
            ScryptEncoder encoder = new ScryptEncoder();
            //TODO GetPasswordHashByLoginAsync
            string passwordHash = context.GetString
                .FromSqlRaw("select napp.get_UserPasswordHash(@Login) as Value", login)
                .FirstOrDefault()
                ?.Value;

            if (!encoder.Compare(auth.Password, passwordHash))
            {
                ModelState.AddModelError("Password", "Неверный пароль");
                return View();
            }

            //TODO GetUserAsync
            User user = context.Users.FromSqlRaw("select * from napp.get_User(@UserID, @Login)", uid, login)
                    .FirstOrDefault();

            //TODO GetAllRolesAsync
            data.RolesMatchings = await context.RolesAndMatchings
                    .FromSqlRaw("select * from napp.get_UserRolesMatchings(@UserID, null)",
                        new SqlParameter("@UserID", user.UserID)).ToListAsync();

            List<string> roles = data.RolesMatchings.Select(item => item.RoleName).ToList();
            await Authenticate(user.Login, data.RolesMatchings.Select(item => item.RoleName).ToList());

            data.SelectedRole = data.RolesMatchings[0].RoleName;
            HttpContext.Response.Cookies.Append("selectedRole", data.RolesMatchings[0].RoleName);
            data.User = user;
            data.SelectedMatching = data.RolesMatchings[0].MatchingID;
            HttpContext.Response.Cookies.Append("selectedMatching",
                data.RolesMatchings[0].MatchingID?.ToString());

            await context.SetLastVisitDate(user.UserID, data.RolesMatchings[0].RoleName,
                data.RolesMatchings[0].MatchingID);

            data.CurrentStage = await context.GetCurrentStageAsync(data.SelectedMatching);

            HttpContext.Session.Set<SessionData>("Data", data);
            return RedirectToLk(data.RolesMatchings[0].RoleName);
        }

        [NonAction]
        private async Task Authenticate(string login, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            }

            var identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login", "Home");
        }

        [NonAction]
        private IActionResult RedirectToLk(string role)
        {
            return role switch
            {
                "Tutor" => RedirectToAction("index", "tutor"),
                "Executive" => RedirectToAction("index", "executive"),
                "Student" => RedirectToAction("index", "student"),
                _ => NotFound()
            };
        }
    }
}