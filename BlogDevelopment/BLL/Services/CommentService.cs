using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.DAL.Reposytoryes;
using Microsoft.EntityFrameworkCore;

namespace BlogDevelopment.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;

        public CommentService(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAll().Include(c => c.User).Include(c => c.Article).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _commentRepository.GetAll()
                .Include(c => c.User)
                .Include(c => c.Article)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(Comment comment)
        {
            await _commentRepository.CreateAsync(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment != null)
            {
                _commentRepository.Delete(comment);
                await _commentRepository.SaveChangesAsync();
            }
        }
    }
}
