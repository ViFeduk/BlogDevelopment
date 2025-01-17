using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.Models.ViewModels.editModels
{
    public class EditTagViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название тега обязательно.")]
        [StringLength(50, ErrorMessage = "Название тега не может быть длиннее 50 символов.")]
        public string Name { get; set; } // Название тега
    }
}
