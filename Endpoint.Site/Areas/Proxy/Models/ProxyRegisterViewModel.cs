using Radin.Common.Dto;
using System.ComponentModel.DataAnnotations;

namespace Endpoint.Site.Areas.Proxy.Models
{
    public class ProxyRegisterViewModel
    {
        [Display(Name = "تلفن همراه")]
        public string phone { get; set; }
    }
}
