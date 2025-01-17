using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Имя пользователя обязательно.")]
        [StringLength(50, ErrorMessage = "Имя пользователя не может быть длиннее 50 символов.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        [StringLength(100, ErrorMessage = "Email не может быть длиннее 100 символов.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля обязательно.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
