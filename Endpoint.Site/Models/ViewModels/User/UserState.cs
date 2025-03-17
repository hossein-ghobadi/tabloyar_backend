namespace Endpoint.Site.Models.ViewModels.User
{
    public class UserState
    {
        public string Email { get; set; }   
        public string Name { get; set; }
        public List<string> Role { get; set; }
        public bool IsSuccess { get; set; }
        public string FullName { get; set; }
        public  long BranchCode { get; set; }
    }
}
