namespace BlogDevelopment.Models.ViewModels
{
    public class ArticleProfileViewModel
    {
        
        public int Id { get; set; } // Оставляем Id только для связи
        public string Title { get; set; }
        public string Description { get; set; }
        public List<TagViewModel> Tags { get; set; } // Список доступных тегов
    }
}
