using BlogDevelopment.BLL.BusinesModels;
using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.Models.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public ArticleViewModel()
        {
            Tags = new List<TagViewModel>();
            SelectedTagIds = new List<int>();
        }
        [Required(ErrorMessage = "Название статьи обязательно")]
        [StringLength(200, ErrorMessage = "Длина названия статьи не должна превышать 200 символов.")]
        public string Title { get; set; }

        [StringLength(30000, ErrorMessage = "Длина названия не должна превышать 30000 символов")]
        public string Description { get; set; }

        public List<TagViewModel> Tags { get; set; } // Список доступных тегов
        public List<int> SelectedTagIds { get; set; } // Список ID выбранных тегов
        public List<CommentViewModel> Comments { get; set; }
    }
    public class CommentViewModel
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
