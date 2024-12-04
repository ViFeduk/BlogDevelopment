using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogDevelopment.BLL.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(IUserService userService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users); // Возвращает представление со списком пользователей
        }

        // Отображение формы регистрации пользователя
        [Route("Register")]
        public IActionResult Register()
        {
            return View(); // Возвращает пустую форму для регистрации
        }

        // Обработка данных регистрации
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(ApplicationUser user, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Назначаем роль "Пользователь"
                    await _userManager.AddToRoleAsync(user, "Пользователь");

                    // Вход пользователя после регистрации
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");  // Перенаправление на главную страницу
                }
                else
                {
                    // Обработка ошибок
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(user);  // Если модель невалидна или ошибка регистрации, возвращаемся к форме
        }

        // Отображение формы редактирования пользователя
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user); // Возвращает форму для редактирования
        }

        // Обработка данных редактирования
        [HttpPut]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateUserAsync(user);
                TempData["Message"] = "User updated successfully!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Failed to update user.";
            return View(user); // Возвращает форму с ошибками валидации
        }

        // Удаление пользователя
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userService.DeleteUserAsync(id);
            TempData["Message"] = "User deleted successfully!";
            return RedirectToAction("Index");
        }

        // Просмотр деталей пользователя
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user); // Возвращает представление с деталями пользователя
        }
    }
}
