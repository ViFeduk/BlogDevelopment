using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.DAL.Reposytoryes;
using Microsoft.EntityFrameworkCore;

namespace BlogDevelopment.BLL.Services
{
    public class ArticleService
    {
        private readonly IRepository<Article> _articleRepository;

        public ArticleService(IRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _articleRepository.GetAll().ToListAsync(); 
        }

        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _articleRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Article>> GetByUserIdAsync(string userId)
        {
            return await _articleRepository.GetAll()
                .Where(article => article.UserId == userId)
                .ToListAsync();
        }

        public async Task CreateAsync(Article article)
        {
            await _articleRepository.CreateAsync(article);
            await _articleRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(Article article)
        {
            _articleRepository.Update(article);
            await _articleRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var article = await _articleRepository.GetByIdAsync(id);
            if (article != null)
            {
                _articleRepository.Delete(article);
                await _articleRepository.SaveChangesAsync();
            }
        }
    }
}
