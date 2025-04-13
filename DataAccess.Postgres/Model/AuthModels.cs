using System.ComponentModel.DataAnnotations;

namespace Web_Api.Model
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Требуется ввести логин.")]
        [MaxLength(30)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Требуется электронная почта.")]
        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты.")]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Требуется ввести пароль.")]
        [MinLength(6, ErrorMessage = "Пароль должен содержать не менее 6 символов.")]
        public string Password { get; set; }
    }

    public class LoginModels
    {
        [Required(ErrorMessage = "Введите Email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль!"),
         MinLength(6, ErrorMessage = "Пароль должен содержать не менее 6 символов.")]
        public string Password { get; set; }
    }
}
