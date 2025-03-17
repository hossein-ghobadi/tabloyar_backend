using Radin.Common.Dto;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Endpoint.Site.Models.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "ایمیل خود را وارد نمایید")]
        //[EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "پسورد خود را وارد نمایید")]
        [DataType(DataType.Password)]
        [Display(Name = "پسورد")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set;} = false;

        public string ReturnUrl { get; set; }

        public List<IdLabelDto> Validate()
        {
            var validationErrors = new List<IdLabelDto>();
            int id = 0;
            if (string.IsNullOrWhiteSpace(Email))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "ایمیل را وارد نمایید"
                });
            }
            // Additional validation for other properties...

            return validationErrors;
        }


    }
}
