using MatchingSystem.DataLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrypt;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MatchingSystem.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;
        private SessionData data;

        public AccountController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            if (data == null)
            {
                data = HttpContext.Session.Get<SessionData>("Data");
            }
        }

        [HttpPost]
        public IActionResult ChangePassword([FromForm] string newPassword)
        {
            var encoder = new ScryptEncoder();
            var passwordHash = encoder.Encode(newPassword);
            
            userRepository.UpdatePasswordHash(data.User.UserID, passwordHash);

            return RedirectToAction("login", "Home");
        }

        public IActionResult ChangeLk(int matchingId, string roleName)
        {
            data.SelectedMatching = matchingId;
            data.SelectedRole = roleName;
            HttpContext.Response.Cookies.Append("selectedRole", roleName);
            HttpContext.Response.Cookies.Append("selectedMatching", matchingId.ToString());

            HttpContext.Session.Set<SessionData>("Data", data);

            userRepository.SetLastVisitDate(data.User.UserID, data.SelectedRole, data.SelectedMatching);

            return roleName switch
            {
                "Tutor" => RedirectToAction("index", "tutor"),
                "Executive" => RedirectToAction("index", "executive"),
                "Student" => RedirectToAction("index", "student"),
                _ => NotFound()
            };
        }
    }
}