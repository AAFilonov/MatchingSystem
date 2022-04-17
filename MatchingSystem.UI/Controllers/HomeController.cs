using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Scrypt;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using MatchingSystem.UI.ViewModels;

namespace MatchingSystem.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IMatchingRepository matchingRepository;

        public HomeController(IUserRepository userRepository, IMatchingRepository matchingRepository)
        {
            this.userRepository = userRepository;
            this.matchingRepository = matchingRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            SessionData data = new SessionData();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = userRepository.GetUser(HttpContext.User.Identity.Name);

                data.RolesMatchings = userRepository.GetAllRoles(user.UserID);

                data.User = user;
                data.CountRoles = data.RolesMatchings.Count();


                var selectedMatching = Convert.ToInt32(HttpContext.Request.Cookies["selectedMatching"]);
                data.SelectedMatching = selectedMatching;
                data.MatchingTypeCode = matchingRepository.GetMatchings()
                    .First(matching => matching.MatchingID.Equals(selectedMatching)).MatchingTypeCode;
                string selectedRole = HttpContext.Request.Cookies["selectedRole"];
                data.SelectedRole = selectedRole;

                data.CurrentStage = matchingRepository.GetCurrentStage(data.SelectedMatching);

                userRepository.SetLastVisitDate(user.UserID, selectedRole, System.Convert.ToInt32(selectedMatching));

                HttpContext.Session.Set<SessionData>("Data", data);

                return RedirectToLk(selectedRole);
            }

            AuthViewModel userVm = new AuthViewModel();
            return View(userVm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel auth)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            SessionData data = new SessionData();

            var userId = await userRepository.GetUserIdByLoginAsync(auth.Login);

            if (userId == -1)
            {
                ModelState.AddModelError("login", "Неверный логин");
                return View();
            }

            ScryptEncoder encoder = new ScryptEncoder();

            var passwordHash = await userRepository.GetPasswordHashByLoginAsync(auth.Login);

            if (!encoder.Compare(auth.Password, passwordHash))
            {
                ModelState.AddModelError("Password", "Неверный пароль");
                return View();
            }

            var user = await userRepository.GetUserAsync(auth.Login);
           
            data.RolesMatchings = await userRepository.GetAllRolesAsync(user.UserID);
            data.RolesMatchings = data.RolesMatchings.OrderByDescending(matching => matching.MatchingId);
            string[] roles = data.RolesMatchings
                .Select(roleItem => roleItem.RoleName)
                .ToArray();

            await Authenticate(user.Login, data.RolesMatchings.Select(item => item.RoleName).ToList());

            data.SelectedRole = data.RolesMatchings.First().RoleName;
            HttpContext.Response.Cookies.Append("selectedRole", data.RolesMatchings.FirstOrDefault()?.RoleName!);
            data.User = user;
            data.SelectedMatching = data.RolesMatchings.First().MatchingId;
            HttpContext.Response.Cookies.Append("selectedMatching",
                data.RolesMatchings.First().MatchingId.ToString()!);
           
            await userRepository.SetLastVisitDateAsync(user.UserID, data.RolesMatchings.First().RoleName!,
                data.RolesMatchings.First().MatchingId);

            data.CurrentStage = await matchingRepository.GetCurrentStageAsync(data.SelectedMatching);
            
            data.MatchingTypeCode = matchingRepository.GetMatchings()
                .First(matching => matching.MatchingID.Equals(data.SelectedMatching)).MatchingTypeCode;
            
            HttpContext.Session.Set<SessionData>("Data", data);
            return RedirectToLk(data.RolesMatchings.First().RoleName);
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