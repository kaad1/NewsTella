namespace NewsTella.Models.ViewModel
{
    public class UserEditVM
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public IList<string> SelectedRoles { get; set; }
    }
}
