using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogDevelopment.BLL.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;

        public AuthenticationController(IUserService userService, SignInManager<UserModel> signInManager, UserManager<UserModel> userManager)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Метод для входа в систему
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();  // Отображаем представление для входа
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model.Email, model.Password);

                if (result)
                {
                    // Перенаправление после успешного входа
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Ошибка: неверные данные
                    ModelState.AddModelError("", "Не удалось войти в систему. Проверьте свои данные.");
                }
            }

            return View(model);  // Если модель невалидна или неудачная попытка входа, возвращаемся на страницу входа
        }

        // Метод для выхода из системы
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Метод для отображения страницы профиля (если нужно)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);  // Получаем текущего пользователя
            return View(user);  // Передаем в представление пользователя для отображения
        }
    }
}
