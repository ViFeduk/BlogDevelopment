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
            Comments = new List<CommentViewModel>(); // Новый список для комментариев
        }

        [Required(ErrorMessage = "Название статьи обязательно")]
        public string Title { get; set; }

        [StringLength(300, ErrorMessage = "Длина названия не должна превышать 300 символов")]
        public string Description { get; set; }

        public List<TagViewModel> Tags { get; set; }
        public List<int> SelectedTagIds { get; set; }

        // Новый список для комментариев
        public List<CommentViewModel> Comments { get; set; }
    }

    public class CommentViewModel
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
