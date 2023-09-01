using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using MatchingSystem.DataLayer.Feature.Matching.service;
using MatchingSystem.DataLayer.Feature.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Scrypt;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using MatchingSystem.UI.ViewModels;
using Microsoft.Extensions.Logging;
using IUserRepository = MatchingSystem.DataLayer.Feature._Interface.IUserRepository;

namespace MatchingSystem.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository userRepository;
        private DataLayer.Feature.User.IUserRepository _userRepository;
        private IMatchingRepository matchingRepository;
        private readonly ILogger<HomeController> logger;

        public HomeController(
            IUserRepository userRepository, 
            IMatchingRepository matchingRepository, 
            DataLayer.Feature.User.IUserRepository userRepository2,  
            ILogger<HomeController> logger)
        {
            this.userRepository = userRepository;
           this.matchingRepository = matchingRepository;
            _userRepository = userRepository2;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            SessionData data = new SessionData();
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var userModel = _userRepository.getByLogin(HttpContext.User.Identity.Name);
                    var user =  UserDto.fromEntity(userModel);

                    data.RolesMatchings = userRepository.GetAllRoles(user.UserId);

                    data.UserDto = user;
                    data.CountRoles = data.RolesMatchings.Count();


                    var selectedMatchingId = Convert.ToInt32(HttpContext.Request.Cookies["selectedMatching"]);

                    if (matchingRepository.GetMatchings()
                        .Any(matching => matching.MatchingId.Equals(selectedMatchingId)))
                    {
                        data.SelectedMatching = selectedMatchingId;
                        data.MatchingTypeCode = matchingRepository.GetMatchings()
                            .First(matching => matching.MatchingId == selectedMatchingId)
                            .MatchingType.MatchingTypeCode;
                        string selectedRole = HttpContext.Request.Cookies["selectedRole"];
                        data.SelectedRole = selectedRole;

                        data.CurrentStage = matchingRepository.GetCurrentStageDto(data.SelectedMatching);

                        userRepository.SetLastVisitDate(user.UserId, selectedRole,
                            System.Convert.ToInt32(selectedMatchingId));

                        HttpContext.Session.Set("Data", data);
                        return RedirectToLk(selectedRole);

                    }
                }
            }
            catch  (Exception e)
            {
                logger.Log(LogLevel.Debug,e.ToString());
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
            User user = null;

            try
            {
                 user = _userRepository.getByLogin(auth.Login);
               
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, "Error during login process: " + e.Message, e);
                logger.Log(LogLevel.Error, e.StackTrace);
                ModelState.AddModelError("login", "Произошла ошибка");
                return View();
            }
          
            if (user == null)
            {
                ModelState.AddModelError("login", "Неверный логин");
                return View();
            }

            ScryptEncoder encoder = new ScryptEncoder();
            

            if (!encoder.Compare(auth.Password, user.PasswordHash))
            {
                ModelState.AddModelError("Password", "Неверный пароль");
                return View();
            }
            
           
            data.RolesMatchings =  userRepository.GetAllRoles(user.UserId);
            data.RolesMatchings = data.RolesMatchings.OrderByDescending(matching => matching.MatchingId);
            string[] roles = data.RolesMatchings
                .Select(roleItem => roleItem.RoleName)
                .ToArray();

            await Authenticate(user.Login, data.RolesMatchings.Select(item => item.RoleName).ToList());

            data.SelectedRole = data.RolesMatchings.First().RoleName;
            HttpContext.Response.Cookies.Append("selectedRole", data.RolesMatchings.FirstOrDefault()?.RoleName!);
            data.UserDto = UserDto.fromEntity(user);
            data.SelectedMatching = data.RolesMatchings.First().MatchingId;
            HttpContext.Response.Cookies.Append("selectedMatching",
                data.RolesMatchings.First().MatchingId.ToString()!);
           
             userRepository.SetLastVisitDate(user.UserId, data.RolesMatchings.First().RoleName!,
                data.RolesMatchings.First().MatchingId);

            data.CurrentStage = matchingRepository.GetCurrentStageDto(data.SelectedMatching);
            
            data.MatchingTypeCode = matchingRepository.GetMatchings()
                .First(matching => matching.MatchingId.Equals(data.SelectedMatching)).MatchingType.MatchingTypeCode;
            
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