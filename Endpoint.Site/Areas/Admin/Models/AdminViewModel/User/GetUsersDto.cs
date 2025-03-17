using Endpoint.Site.Areas.Admin.Models.AdminViewModel.Role;
using Radin.Common.Dto;

namespace Endpoint.Site.Areas.Admin.Models.AdminViewModel.User
{
    public class GetUsersDto
    {
        public string username { get; set; }
        public string email { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string phone2 { get; set; }
        public string? gender { get; set; }
        public string? job { get; set; }
        public IdLabelDto? state { get; set; }
        public IdLabelDto? city { get; set; }
        public long? age { get; set; }
        public string? address { get; set; }
        public bool IsActive { get; set; }
        public bool IsRemove { get; set; }
        public List<RoleNameGetDto> role  { get; set; }
        public long BranchCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


    }
}
