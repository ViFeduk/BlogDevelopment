using BlogDevelopment.DAL.DataBase;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlogDevelopment.DAL.Reposytoryes
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbContext _db;
        protected readonly DbSet<T> Set;

        public BaseRepository(MyAppContext db)
        {
            _db = db;
            Set = _db.Set<T>();
        }


        public IQueryable<T> GetAll()
        {
            return Set.AsQueryable();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task CreateAsync(T item)
        {
            await Set.AddAsync(item);
        }

        public void Update(T item)
        {
            Set.Update(item);
        }

        public void Delete(T item)
        {
            Set.Remove(item);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
