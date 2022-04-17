using System.Linq;
using MatchingSystem.DataLayer.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMatchingRepository matchingRepository;

        public UserController(IUserRepository userRepository, IMatchingRepository matchingRepository)
        {
            this.matchingRepository = matchingRepository;
            this.userRepository = userRepository;
        }

        [Route("api/[controller]/getMatchings")]
        [HttpGet]
        public IActionResult GetMatchingsForUser([FromQuery] int userId)
        {
            var model = matchingRepository.GetMatchingsByUser(userId).OrderByDescending(matching =>matching.MatchingID );
            return new JsonResult(model);
        }

        [Route("api/[controller]/getRoles")]
        [HttpGet]
        public IActionResult GetRolesForUser([FromQuery] int matchingId, [FromQuery] int userId)
        {
            var model = userRepository.GetRolesForUserAndMatching(userId, matchingId);
            
            return new JsonResult(model);
        }
    }
}