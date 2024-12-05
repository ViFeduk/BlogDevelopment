using BlogDevelopment.BLL.BusinesModels;

namespace BlogDevelopment.BLL.Services.Intarface
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllAsync(); // Получить все теги
        Task<Tag?> GetByIdAsync(int id); // Получить тег по ID
        Task CreateAsync(Tag tag); // Создать тег
        Task UpdateAsync(Tag tag); // Обновить тег
        Task DeleteAsync(int id); // Удалить тег
    }
}
