using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Group.repo;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.Role.repo;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.Feature.User.repo;
using MatchingSystem.Service.Matching;
using Microsoft.AspNetCore.Mvc;
using Scrypt;

namespace MatchingSystem.UI.ApiControllers
{
    public class ChangePasswordRequest
    {
        public UserDto User { get; set; }
        public string NewPassword { get; set; }
    }

    public class AssignRoleRequest
    {
        public UsersRoleEntity userRoleEntity { get; set; }
        public int groupId { get; set; }
    }

    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMatchingRepository matchingRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IMatchingService matchingService;
        private readonly IGroupRepository groupRepository;
        private readonly IStudentRepository studentRepository;

        public AdminController(IUserRepository userRepository, IMatchingRepository matchingRepository,
            IRoleRepository roleRepository, IMatchingService matchingService, IGroupRepository groupRepository,
            IStudentRepository studentRepository)
        {
            this.userRepository = userRepository;
            this.matchingRepository = matchingRepository;
            this.roleRepository = roleRepository;
            this.matchingService = matchingService;
            this.groupRepository = groupRepository;
            this.studentRepository = studentRepository;
        }

        [Route("api/[controller]/getMatchingsList")]
        [HttpGet]
        public IActionResult GetMatchingsList()
        {
            var model = matchingRepository.GetMatchingsDto().ToList();
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

        [Route("api/[controller]/assignRole")]
        [HttpPost]
        public void AssignRole(AssignRoleRequest payload)
        {
            IEnumerable<UsersRoleEntity> userRole = roleRepository.find(role =>
                role.UserId == payload.userRoleEntity.UserId
                && role.MatchingId == payload.userRoleEntity.MatchingId
                && role.RoleId == payload.userRoleEntity.RoleId
            );
            
            if (userRole.Count() != 0 && userRole.First().IsDeleted == false)
            {
                throw new Exception("Роль с такой конфигурацией уже существует");
            }

            // RoleId = Student
            if (payload.userRoleEntity.RoleId == (int)RolesEnum.STUDENT && payload.groupId != null)
            {
                roleRepository.CallInTransaction((context) =>
                {
                    StudentEntity newStudentEntity = new StudentEntity
                    {
                        GroupId = payload.groupId,
                        MatchingId = payload.userRoleEntity.MatchingId,
                    };
                    studentRepository.save(newStudentEntity, context);

                    var possibleStudentRole = roleRepository.find(role =>
                        role.MatchingId == payload.userRoleEntity.MatchingId
                        && role.RoleId == (int)RolesEnum.STUDENT
                        && role.UserId == payload.userRoleEntity.UserId
                    ).FirstOrDefault();

                    if (possibleStudentRole != null)
                    {
                        possibleStudentRole.IsDeleted = false;
                        roleRepository.update(possibleStudentRole, context);
                    }
                    else
                    {
                        var studentRole = new UsersRoleEntity
                        {
                            MatchingId = payload.userRoleEntity.MatchingId,
                            RoleId = (int)RolesEnum.STUDENT,
                            UserId = payload.userRoleEntity.UserId,
                            StudentId = newStudentEntity.StudentId,
                        };
                    
                        roleRepository.save(studentRole, context);
                    }
                });
            }
            else // RoleId ни студент, ни преподаватель
            {
                // Создание записи в User_Roles
                roleRepository.AssignRoleForUser(payload.userRoleEntity.UserId, (int)payload.userRoleEntity.MatchingId,
                    payload.userRoleEntity.RoleId);
            }
        }

        [Route("api/[controller]/removeRole")]
        [HttpPost]
        public void RemoveRole(UsersRoleEntity payload)
        {
            if (payload.RoleId == (int)RolesEnum.STUDENT)
            {
                UsersRoleEntity userRole = roleRepository.find(role =>
                    role.UserId == payload.UserId
                    && role.MatchingId == payload.MatchingId
                    && role.RoleId == payload.RoleId
                ).Single();
                userRole.IsDeleted = true;
                roleRepository.update(userRole);
            }
            else
            {
                UsersRoleEntity userRole = roleRepository.find(role =>
                    role.UserId == payload.UserId
                    && role.MatchingId == payload.MatchingId
                    && role.RoleId == payload.RoleId
                ).Single();
                roleRepository.remove(userRole);
            }
        }
    }
}