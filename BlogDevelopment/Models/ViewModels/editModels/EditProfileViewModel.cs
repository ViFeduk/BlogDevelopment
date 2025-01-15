using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.Models.ViewModels.editModels
{
    public class EditProfileViewModel
    {
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Некорректный формат номера телефона.")]
        public string PhoneNumber { get; set; }

        public List<RoleSelectionViewModel> Roles { get; set; }
    }

    public class RoleSelectionViewModel
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
