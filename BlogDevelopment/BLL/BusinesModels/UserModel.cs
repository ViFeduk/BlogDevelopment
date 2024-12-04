using Microsoft.AspNetCore.Identity;

namespace BlogDevelopment.BLL.BusinesModels
{
    public class UserModel: IdentityUser
    {

        public string FirstName { get; set; }
        public string? MidleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }

    }
}
