using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.DAL.Reposytoryes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogDevelopment.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IRepository<UserModel> _userRepository;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public UserService(IRepository<UserModel> userRepository,
                           UserManager<UserModel> userManager,
                           SignInManager<UserModel> signInManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> LoginUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return false; // Пользователь не найден
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (result.Succeeded)
            {
                return true;  // Успешный вход
            }
            else if (result.IsLockedOut)
            {
                // Логика на случай блокировки аккаунта
                return false; // Аккаунт заблокирован
            }
            else
            {
                return false; // Неверный пароль
            }
        }
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                _userRepository.Delete(user);
                await _userRepository.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _userRepository.GetAll().ToListAsync();
        }

        public async Task<UserModel?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task RegisterUserAsync(UserModel user)
        {
            await _userRepository.CreateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserModel user)
        {
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
