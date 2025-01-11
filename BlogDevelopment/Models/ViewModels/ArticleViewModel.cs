using BlogDevelopment.BLL.BusinesModels;
using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.Models.ViewModels
{
    public class ArticleViewModel
    {
        public ArticleViewModel()
        {
            Tags = new List<TagViewModel>();
            SelectedTagIds = new List<int>();
        }
        [Required(ErrorMessage = "Название статьи обязательно")]
        public string Title { get; set; }

        [StringLength(300, ErrorMessage = "Длина названия не должна превышать 300 символов")]
        public string Description { get; set; }

        public List<TagViewModel> Tags { get; set; } // Список доступных тегов
        public List<int> SelectedTagIds { get; set; } // Список ID выбранных тегов
    }
}
