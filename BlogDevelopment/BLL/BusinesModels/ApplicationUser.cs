using Microsoft.AspNetCore.Identity;

namespace BlogDevelopment.BLL.BusinesModels
{
    public class ApplicationUser : IdentityUser
    {
        // Дополнительные свойства пользователя, если нужно
        public ICollection<Article> Articles { get; set; }  // Связь с статьями
        public ICollection<Comment> Comments { get; set; }  // Связь с комментариями
    }
}
