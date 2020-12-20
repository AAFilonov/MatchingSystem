using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Scrypt;
using MatchingSystem.UI.Helpers;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MatchingSystem.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DataContext context;
        private SessionData data;

        public AccountController(DataContext ctx)
        {
            context = ctx;
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
        public async Task<IActionResult> ChangePassword([FromForm] string newPassword)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            string passwordhash = encoder.Encode(newPassword);

            SqlParameter userID = new SqlParameter("@UserID", data.User?.UserID);
            SqlParameter hash = new SqlParameter("@NewPasswordHash", passwordhash);

            //TODO UpdatePasswordHashAsync
            await context.Database.ExecuteSqlRawAsync("execute napp.upd_User_PasswordHash @UserID, @NewPasswordHash", userID, hash);

            return RedirectToAction("login", "Home");
        }

        public async Task<IActionResult> ChangeLk(int matchingID, string roleName)
        {
            data.SelectedMatching = matchingID;
            data.SelectedRole = roleName;
            HttpContext.Response.Cookies.Append("selectedRole", roleName);
            HttpContext.Response.Cookies.Append("selectedMatching", matchingID.ToString());

            HttpContext.Session.Set<SessionData>("Data", data);

            await context.SetLastVisitDate(data.User.UserID, roleName, matchingID);

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