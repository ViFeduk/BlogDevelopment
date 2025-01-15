using BlogDevelopment.BLL.BusinesModels;

namespace BlogDevelopment.BLL.Services.Intarface
{
    public interface IUserService
    {
        Task RegisterUserAsync(ApplicationUser user);

        // Редактирование данных пользователя
        Task UpdateUserAsync(ApplicationUser user);

        // Удаление пользователя
        Task DeleteUserAsync(string userId);

        // Получение всех пользователей
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        // Получение пользователя по идентификатору
        Task<ApplicationUser?> GetUserByIdAsync(string userId);

        Task<bool> LoginUserAsync(string username, string password);
    }
}
