using System.Linq;
using Lombok.NET;
using MatchingSystem.DataLayer.Feature.Group.repo;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.Role.repo;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.Feature.User.repo;
using MatchingSystem.Service.Admin;
using Microsoft.AspNetCore.Mvc;
using Scrypt;

namespace MatchingSystem.UI.ApiControllers
{
    public class ChangePasswordRequest
    {
        public UserDto User { get; set; }
        public string NewPassword { get; set; }
    }

    [ApiController]
    [AllArgsConstructor]
    public partial class AdminController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMatchingRepository matchingRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IAdminService adminService;

        [Route("api/[controller]/getMatchingsList")]
        [HttpGet]
        public IActionResult GetMatchingsList()
        {
            var model = matchingRepository
                .findAll()
                .Select(entity => new MatchingDto(entity))
                .ToList();
            return new JsonResult(model);
        }

        [Route("api/[controller]/getRolesList")]
        [HttpGet]
        public IActionResult GetRolesList()
        {
            var model = roleRepository.GetRoles();
            return new JsonResult(model);
        }

        [Route("api/[controller]/getGroupsList")]
        [HttpGet]
        public IActionResult GetGroupsList()
        {
            var model = groupRepository.GetGroups();
            return new JsonResult(model);
        }

        [Route("api/[controller]/getAllRoles")]
        [HttpGet]
        public IActionResult GetAllRolesForUser([FromQuery] int userId)
        {
            var model = roleRepository.GetRolesForUser(userId);
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

        [Route("api/[controller]/updateUser")]
        [HttpPost]
        public void UpdateUser(UserDto userDto)
        {
            UserEntity user = userRepository.findById(userDto.UserId);
            user.Email = userDto.Email;
            user.Login = userDto.Login;
            user.Name = userDto.Name;
            user.Surname = userDto.Surname;
            user.Patronimic = userDto.Patronimic;
            userRepository.update(user);
        }

        [Route("api/[controller]/changePassword")]
        [HttpPost]
        public void ChangePassword(ChangePasswordRequest payload)
        {
            var encoder = new ScryptEncoder();
            var passwordHash = encoder.Encode(payload.NewPassword);
            UserEntity user = userRepository.findById(payload.User.UserId);
            user.PasswordHash = passwordHash;
            userRepository.update(user);
        }

        [Route("api/[controller]/assignDefaultRole")]
        [HttpPost]
        public void AssignDefaultRole(AssignDefaultRoleRequest payload)
        {
            adminService.AssignDefaultRole(payload);
        }

        [Route("api/[controller]/assignStudentRole")]
        [HttpPost]
        public void AssignStudentRole(AssignStudentRoleRequest payload)
        {
            adminService.AssignStudentRole(payload);
        }

        [Route("api/[controller]/assignTutorRole")]
        [HttpPost]
        public void AssignTutorRole(AssignTutorRoleRequest payload)
        {
            adminService.AssignTutorRole(payload);
        }

        [Route("api/[controller]/removeRole")]
        [HttpPost]
        public void RemoveRole(RemoveRoleRequest payload)
        {
            switch (payload.User.RoleId)
            {
                case (int)RolesEnum.STUDENT: adminService.RemoveStudentRole(payload); break;
                case (int)RolesEnum.TUTOR: adminService.RemoveTutorRole(payload); break;
                default: adminService.RemoveDefaultRole(payload); break;
            }
        }
    }
}