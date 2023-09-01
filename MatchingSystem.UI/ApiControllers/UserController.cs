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

        public UserController(IUserRepository userRepository, IMatchingRepository matchingRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.matchingRepository = matchingRepository;
            this.roleRepository = roleRepository;
        }

        [Route("api/[controller]/getMatchings")]
        [HttpGet]
        public IActionResult GetMatchingsForUser([FromQuery] int userId)
        {
            var model = matchingRepository.GetMatchingsByUser(userId);
            return new JsonResult(model);
        }

        [Route("api/[controller]/getRoles")]
        [HttpGet]
        public IActionResult GetRolesForUser([FromQuery] int matchingId, [FromQuery] int userId)
        {
            var model = roleRepository.GetRolesForUser(userId);
            return new JsonResult(model);
        }
    }
}