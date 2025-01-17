using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.Models.ViewModels.editModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Имя пользователя обязательно.")]
        [StringLength(50, ErrorMessage = "Имя пользователя не может быть длиннее 50 символов.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Некорректный формат номера телефона.")]
        [StringLength(15, ErrorMessage = "Номер телефона не может быть длиннее 15 символов.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать хотя бы одну роль.")]
        public List<RoleSelectionViewModel> Roles { get; set; }
    }

    public class RoleSelectionViewModel
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
