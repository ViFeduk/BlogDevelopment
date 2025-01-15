namespace BlogDevelopment.Models.ViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } // Список ролей пользователя
    }
}
