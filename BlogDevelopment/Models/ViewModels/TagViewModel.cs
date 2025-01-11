using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.Models.ViewModels
{
    public class TagViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название тега обязательно")]
        [StringLength(50, ErrorMessage = "Длина названия не должна превышать 50 символов")]
        public string Name { get; set; } // Для ввода нового тега
        public bool IsSelected { get; set; } // Флаг, выбран ли тег
    }
}
