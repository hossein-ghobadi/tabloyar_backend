using Endpoint.Site.Models.ViewModels.Register;
using Radin.Common.Dto;
using System.ComponentModel.DataAnnotations;

namespace Endpoint.Site.Models.ViewModels.User
{
    public class FinalRegisterStep
    {
        [Display(Name = "ایمیل")]
        public string email { get; set; }

        [Display(Name = "نام کاربری")]
        public string name { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "پسورد")]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = " تکرار پسورد")]
        public string rePassword { get; set; }

        [Display(Name = "نام کامل")]
        public string fullName { get; set; }

        [Display(Name = "تلفن همراه")]
        public string phone { get; set; }

        [Display(Name = " کد تایید")]
        public string verifyCode {  get; set; }

    }
}
