namespace ASP.NET_FinalProject.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IList<string> Roles { get; set; } = new List<string>();
        public bool IsAdmin { get; set; }
    }
}
