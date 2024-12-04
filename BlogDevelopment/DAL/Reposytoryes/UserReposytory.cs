using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.DAL.DataBase;
using Microsoft.EntityFrameworkCore;

namespace BlogDevelopment.DAL.Reposytoryes
{
    public class UserReposytory: BaseRepository<ApplicationUser>
    {

        public UserReposytory(MyAppContext db) : base(db)
        {
            
        }
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await Set.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
