using BlogDevelopment.BLL.BusinesModels;

namespace BlogDevelopment.BLL.Services.Intarface
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllAsync(); // Получить все комментарии
        Task<Comment?> GetByIdAsync(int id); // Получить комментарий по ID
        Task CreateAsync(Comment comment); // Создать комментарий
        Task UpdateAsync(Comment comment); // Обновить комментарий
        Task DeleteAsync(int id); // Удалить комментарий
    }
}
