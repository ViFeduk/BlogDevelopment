using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Логин обязателен.")]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        [Display(Name = "Логин")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }

}
