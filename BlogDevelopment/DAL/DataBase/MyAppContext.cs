using BlogDevelopment.BLL.BusinesModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogDevelopment.DAL.DataBase
{
    public class MyAppContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        public MyAppContext(DbContextOptions<MyAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Связь многие ко многим между Article и Tag через PostTag
            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.ArticleId, pt.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Article)
                .WithMany(a => a.PostTags)
                .HasForeignKey(pt => pt.ArticleId);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);
        }

    }
}
