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
        [Display(Name = "Логин")]
        public string Login { get; set; }
        #nullable enable
        public DateTime? LastVisitDate { get; set; }
        [Display(Name = "Email")]
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronimic { get; set; }
        [Display(Name = "ФИО")]
        public string? NameAbbreviation { get; set; }
    }
}
