using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogDevelopment.BLL.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IArticleService _articleService;


        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IArticleService articleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _articleService = articleService;
        }

        // Отображение страницы входа
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Обработка входа пользователя
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        // Роли автоматически подтягиваются, если настроено корректно
                        return RedirectToAction("Profile");
                    }

                    // Если вход не удался
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Ваш аккаунт заблокирован.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь с таким email не найден.");
                }
            }

            return View(model);
        }

        // Логика выхода
        [HttpPost]
        [Route("Logout")]
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Регистрация пользователя
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Обработка регистрации
        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUser model, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Присваиваем роль по умолчанию (например, "Пользователь")
                    await _userManager.AddToRoleAsync(user, "Пользователь");

                    // Вход пользователя после регистрации
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        [Authorize]
        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var roles = await _userManager.GetRolesAsync(user);
           
            var articles = await _articleService.GetByUserIdAsync(user.Id);

            var profileViewModel = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList(),
                Articles = articles.Select(a => new ArticleProfileViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Tags = a.PostTags != null && a.PostTags.Any()
                           ? a.PostTags.Select(at => new TagViewModel
                           {
                               // Предположим, что у каждого тега есть свойство Name
                               Name = at.Tag.Name
                           }).ToList()
                           : new List<TagViewModel>() // Если тегов нет, просто пустой список
                }).ToList()
            };

            return View(profileViewModel);
        }
    }
}
