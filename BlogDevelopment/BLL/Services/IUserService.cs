using BlogDevelopment.BLL.BusinesModels;

namespace BlogDevelopment.BLL.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(ApplicationUser user);

        // Редактирование данных пользователя
        Task UpdateUserAsync(ApplicationUser user);

        // Удаление пользователя
        Task DeleteUserAsync(int userId);

        // Получение всех пользователей
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        // Получение пользователя по идентификатору
        Task<ApplicationUser?> GetUserByIdAsync(int userId);
        Task<bool> LoginUserAsync(string username, string password);
    }
}
