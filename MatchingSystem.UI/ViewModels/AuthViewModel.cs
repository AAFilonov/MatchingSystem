using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchingSystem.UI.ViewModels
{
    public class AuthViewModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Login { get; set; }
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Password { get; set; }
    }
}
