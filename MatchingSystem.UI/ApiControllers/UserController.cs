using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer;
using MatchingSystem.UI.ResultModels;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext context;
        public UserController(DataContext ctx) => context = ctx;
        
        [Route("api/[controller]/getMatchings")]
        [HttpGet]
        public async Task<IActionResult> GetMatchingsForUser([FromQuery] int userId)
        {
            List<Matching> model = new List<Matching>();
            model = await context.GetMatchingsForUserAsync(userId);
            return new JsonResult(model);
        }

        [Route("api/[controller]/getRoles")]
        [HttpGet]
        public async Task<IActionResult> GetRolesForUser([FromQuery] int matchingId, [FromQuery] int userId)
        {
            List<Role> model = new List<Role>();
            model = await context.GetRolesForUserAsync(userId, matchingId);
            
            return new JsonResult(model);
        }
    }
}