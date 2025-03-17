namespace Endpoint.Site.Areas.Admin.Models.AdminViewModel.User
{
    public class UserGetDto
    {
        public string id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsRemove { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public long BranchCode { get; set; }

    }
}
