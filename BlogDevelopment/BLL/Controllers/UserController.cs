using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.Models.ViewModels;
using BlogDevelopment.Models.ViewModels.editModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogDevelopment.BLL.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUserService userService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList(); // Получаем всех пользователей
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Получаем роли для каждого пользователя
                var userViewModel = new UserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = (List<string>)roles
                };

                userViewModels.Add(userViewModel);
            }

            return View(userViewModels); // Передаем список пользователей в представление
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Назначаем роль "Пользователь"
                    await _userManager.AddToRoleAsync(user, "Пользователь");

                    // Вход пользователя после регистрации
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model); // Если не удалось, возвращаем форму с ошибками
        }

        // Отображение формы редактирования пользователя
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            var allRoles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = allRoles.Select(role => new RoleSelectionViewModel
                {
                    RoleName = role.Name,
                    IsSelected = userRoles.Contains(role.Name)
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Обновление ролей пользователя
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.Roles.Where(r => r.IsSelected && !currentRoles.Contains(r.RoleName)).Select(r => r.RoleName).ToList();
            var rolesToRemove = currentRoles.Where(r => !model.Roles.Any(mr => mr.RoleName == r && mr.IsSelected)).ToList();

            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при удалении ролей.");
                return View(model);
            }

            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при добавлении ролей.");
                return View(model);
            }

            return RedirectToAction("Index", "Profile");
        }

        // Удаление пользователя
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userService.DeleteUserAsync(id);
            TempData["Message"] = "User deleted successfully!";
            return RedirectToAction("Index");
        }

        // Просмотр деталей пользователя
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user); // Возвращает представление с деталями пользователя
        }
    }
}
