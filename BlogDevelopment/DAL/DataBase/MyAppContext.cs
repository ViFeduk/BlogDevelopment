using BlogDevelopment.BLL.BusinesModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogDevelopment.DAL.DataBase
{
    public class MyAppContext : IdentityDbContext<UserModel>
    {
        public DbSet<UserModel> User { get; set; }
        public DbSet<ArticleModel> Article { get; set; }
        public DbSet<CommentModel> Comment { get; set; }
        public DbSet<TagModel> Tag { get; set; }


        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options)
        {
        }
        
    }
}
