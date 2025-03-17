namespace Endpoint.Site.Areas.Admin.Models.AdminViewModel.User
{
    public class PageinationModel
    {
        public int PageNumber { get; set; } = 1; // Default to page 1
        public int PageSize { get; set; } = 10;  // Default to 10 items per page
    }
}
