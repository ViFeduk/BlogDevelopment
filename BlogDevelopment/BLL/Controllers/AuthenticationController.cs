using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services;
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

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                        // Получаем роли пользователя
                        var roles = await _userManager.GetRolesAsync(user);

                        // Создаем список клаймов для ролей
                        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

                        // Добавляем клаймы ролей к текущему пользователю
                        var claimsIdentity = new ClaimsIdentity(roleClaims, "login");
                        var principal = new ClaimsPrincipal(claimsIdentity);

                        // Устанавливаем клаймы для текущего пользователя
                        await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);

                        // Применяем клаймы для пользователя
                        HttpContext.User = principal;

                        // Перенаправляем на главную страницу
                        return RedirectToAction("Profile");
                    }

                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
                }

               
            }
            return View();
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
            ViewBag.Roles = roles;

            return View(user);  // Передаем пользователя и его роли в представление
        }
    }
}
