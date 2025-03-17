namespace Endpoint.Site.Areas.Admin.Models.AdminViewModel.Claim
{
    public class RoleClaimUpdate
    {
        public string rolename {  get; set; }
        public List<CategoricalAccess> AccessCategory { get; set; }
    }


    public class CategoricalAccess
    {
        public string id { get; set; }
        public string label { get; set; }
        public List<Access> access { get; set; }
    }

    public class Access
    {
        public string type { get; set; }
        public string description { get; set; }
        public string value { get; set; }

    }


}
