using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.DAL.Reposytoryes;
using Microsoft.EntityFrameworkCore;

namespace BlogDevelopment.BLL.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagRepository;

        public TagService(IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _tagRepository.GetAll().ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _tagRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Tag tag)
        {
            await _tagRepository.CreateAsync(tag);
            await _tagRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag tag)
        {
            _tagRepository.Update(tag);
            await _tagRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag != null)
            {
                _tagRepository.Delete(tag);
                await _tagRepository.SaveChangesAsync();
            }
        }
    }
}
