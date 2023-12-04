using System;
using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.Feature.User
{
    public class UserDto
    {
        public static UserDto fromEntity(UserEntity user)
        {
            var dto = new UserDto
            {
                UserId = user.UserId,
                Login = user.Login,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Patronimic = user.Patronimic,
                NameAbbreviation = user.NameAbbreviation
            };
            return dto;
        }

        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        #nullable enable
        public DateTime? LastVisitDate { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronimic { get; set; }
        public string? NameAbbreviation { get; set; }
    }
}
