using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlogDevelopment.BLL.BusinesModels
{
        public class LoginModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
    }
    
}
