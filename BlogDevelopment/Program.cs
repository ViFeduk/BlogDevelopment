using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.DAL.DataBase;
using BlogDevelopment.DAL.Reposytoryes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

namespace BlogDevelopment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // ���������� ��������� ���� ������
            builder.Services.AddDbContext<MyAppContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefoultConnection")));

            builder.Services.AddControllersWithViews()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/PLL/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/PLL/Views/Shared/{0}.cshtml");
                });

            // ���������� ������������
            builder.Services.AddScoped<IRepository<ApplicationUser>, UserReposytory>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ITagService, TagService>();

            // ���������� Identity � UserManager � RoleManager
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyAppContext>()
                .AddDefaultTokenProviders();

            var app = builder.Build();

            // ������������� Identity � �������� �����, ���� ����������
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                // ����� ����� ��������� ������������� ����� ��� �������������
            }

            // ��������� HTTP-���������
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithRedirects("/Error/{0}"); // ��� 404 � ������ ������
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // ��������� ��������� ������
            app.UseExceptionHandler("/Error/Index"); // ������������ ������ 500

            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
