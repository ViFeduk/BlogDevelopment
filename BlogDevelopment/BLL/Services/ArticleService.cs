using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.DAL.Reposytoryes;
using Microsoft.EntityFrameworkCore;

namespace BlogDevelopment.BLL.Services
{
    public class ArticleService: IArticleService
    {
        private readonly IRepository<Article> _articleRepository;

        public ArticleService(IRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _articleRepository.GetAll().Include(a => a.PostTags)
        .ThenInclude(pt => pt.Tag)
        .ToListAsync();
        }

        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _articleRepository.GetAll()
         .Include(a => a.PostTags)           // Загружаем PostTags
         .ThenInclude(pt => pt.Tag)          // Загружаем Tag из PostTags
         .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Article>> GetByUserIdAsync(string userId)
        {
            return await _articleRepository.GetAll()
        .Include(article => article.PostTags)        // Загружаем PostTags
        .ThenInclude(postTag => postTag.Tag)         // Загружаем Tag из PostTags
        .Where(article => article.UserId == userId) // Фильтруем по UserId
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
            var article = await GetByIdAsync(id);
            if (article != null)
            {
                _articleRepository.Delete(article);
                await _articleRepository.SaveChangesAsync();
            }
        }
    }
}
