using System.Linq;
using MatchingSystem.DataLayer.Feature.Matching.service;
using MatchingSystem.DataLayer.Feature.Role.service;
using MatchingSystem.DataLayer.Feature.User.service;
using Microsoft.AspNetCore.Mvc;

namespace MatchingSystem.UI.ApiControllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMatchingRepository matchingRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IMatchingService matchingService;

        public UserController(IUserRepository userRepository, IMatchingRepository matchingRepository, IRoleRepository roleRepository, IMatchingService matchingService)
        {
            this.userRepository = userRepository;
            this.matchingRepository = matchingRepository;
            this.roleRepository = roleRepository;
            this.matchingService = matchingService;
        }

        [Route("api/[controller]/getMatchings")]
        [HttpGet]
        public IActionResult GetMatchingsForUser([FromQuery] int userId)
        {
            var model = matchingService.GetMatchingsByUser(userId);
            return new JsonResult(model);
        }

        [Route("api/[controller]/getRoles")]
        [HttpGet]
        public IActionResult GetRolesForUser([FromQuery] int matchingId, [FromQuery] int userId)
        {
            var model = roleRepository.GetRolesForUserAndMatching(userId, matchingId);
            return new JsonResult(model);
        }
    }
}