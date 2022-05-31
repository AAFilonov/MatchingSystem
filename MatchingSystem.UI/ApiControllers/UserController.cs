using System.Linq;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.Service.User;
using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route("api/[controller]/getMatchings")]
        [HttpGet]
        public IActionResult GetMatchingsForUser([FromQuery] int userId)
        {
            var model = userService.GetMatchingsForUser(userId);
            return new JsonResult(model);
        }

        [Route("api/[controller]/getRoles")]
        [HttpGet]
        public IActionResult GetRolesForUser([FromQuery] int matchingId, [FromQuery] int userId)
        {
            var model = userService.GetRolesForUser(matchingId, userId);
            return new JsonResult(model);
        }
    }
}