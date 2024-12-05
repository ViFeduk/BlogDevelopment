using BlogDevelopment.BLL.BusinesModels;

namespace BlogDevelopment.BLL.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article?> GetByIdAsync(int id);
        Task<IEnumerable<Article>> GetByUserIdAsync(string userId);
        Task CreateAsync(Article article);
        Task UpdateAsync(Article article);
        Task DeleteAsync(int id);
    }
}
