using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.Models.ViewModels.editModels
{
    public class EditArticleViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Заголовок не может быть длиннее 100 символов.")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Описание не может быть длиннее 500 символов.")]
        public string Description { get; set; }

        // Список доступных тегов
        public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
        public List<int> SelectedTagIds { get; set; } = new List<int>();
    }
}
