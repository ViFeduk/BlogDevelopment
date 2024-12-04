using BlogDevelopment.BLL.BusinesModels;

namespace BlogDevelopment.BLL.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(UserModel user);

        // Редактирование данных пользователя
        Task UpdateUserAsync(UserModel user);

        // Удаление пользователя
        Task DeleteUserAsync(int userId);

        // Получение всех пользователей
        Task<IEnumerable<UserModel>> GetAllUsersAsync();

        // Получение пользователя по идентификатору
        Task<UserModel?> GetUserByIdAsync(int userId);
        Task<bool> LoginUserAsync(string username, string password);
    }
}
