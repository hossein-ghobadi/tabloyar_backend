namespace Endpoint.Site.Areas.Admin.Models.AdminViewModel.User
{
    public class UsersGetInfo
    {
        public int count { get; set; }
        public List<UserGetDto> UsersInfo { get; set; }
        public int PageCount { get; set; }
    }
}
