namespace BlogDevelopment.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public List<ArticleProfileViewModel> Articles { get; set; }
    }
}
