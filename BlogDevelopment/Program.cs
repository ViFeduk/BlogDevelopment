using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services;
using BlogDevelopment.DAL.DataBase;
using BlogDevelopment.DAL.Reposytoryes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogDevelopment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddDbContext<MyAppContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefoultConnection")));
            builder.Services.AddControllersWithViews()

        .AddRazorOptions(options =>
        {
            options.ViewLocationFormats.Clear();
            options.ViewLocationFormats.Add("/PLL/Views/{1}/{0}.cshtml");
            options.ViewLocationFormats.Add("/PLL/Views/Shared/{0}.cshtml");
        });
            builder.Services.AddScoped<IRepository<UserModel>, UserReposytory>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<MyAppContext>()
    .AddDefaultTokenProviders();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
