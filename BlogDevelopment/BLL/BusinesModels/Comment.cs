namespace BlogDevelopment.BLL.BusinesModels
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }

        // Связь с пользователем (автором комментария)
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Связь со статьей
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
