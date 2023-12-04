using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Role.repo;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.Feature.User.repo;
using MatchingSystem.Service.Matching;
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

        public UserController(IUserRepository userRepository, IMatchingRepository matchingRepository,
            IRoleRepository roleRepository, IMatchingService matchingService)
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

        [Route("api/[controller]/getUsers")]
        [HttpGet]
        public IActionResult GetUsers([FromQuery] string abbr = "", [FromQuery] string login = "")
        {
            var model = userRepository.findAll()
                .Where(user =>
                {
                    var nameAbbr = "" + user.NameAbbreviation;
                    if (abbr == null && login == null)
                    {
                        return true;
                    }

                    if (abbr != null && login != null)
                    {
                        return nameAbbr.ToLower().Contains(abbr.ToLower()) &&
                               user.Login.ToLower().Contains(login.ToLower());
                    }

                    if (abbr != null)
                    {
                        return nameAbbr.ToLower().Contains(abbr.ToLower());
                    }

                    return user.Login.ToLower().Contains(login.ToLower());
                })
                .Select(user =>
                {
                    var dto = new UserDto();
                    dto.UserId = user.UserId;
                    dto.Email = user.Email;
                    dto.Login = user.Login;
                    dto.Name = user.Name;
                    dto.Surname = user.Surname;
                    dto.Patronimic = user.Patronimic;
                    dto.NameAbbreviation = user.NameAbbreviation;

                    return dto;
                }).ToList();

            return new JsonResult(model);
        }

        [Route("api/[controller]/getMatchingsList")]
        [HttpGet]
        public IActionResult GetMatchingsList()
        {
            var model = matchingRepository.GetMatchingsDto().ToList();

            return new JsonResult(model);
        }

        [Route("api/[controller]/saveUser")]
        [HttpPost]
        public void saveUsers(UserDto userDto)
        {
            UserEntity user = new UserEntity()
            {
                UserId = userDto.UserId,
                Email = userDto.Email,
                Login = userDto.Login,
                Name = userDto.Name,
                Surname = userDto.Surname,
                Patronimic = userDto.Patronimic,
            };
            // todo починить update
            userRepository.update(user);
        }
    }
}