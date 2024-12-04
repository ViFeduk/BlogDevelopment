namespace BlogDevelopment.BLL.BusinesModels
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // Связь с пользователем (автором статьи)
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Связь с комментариями
        public ICollection<Comment> Comments { get; set; }

        // Связь с тегами
        public ICollection<PostTag> PostTags { get; set; }
    }
}
