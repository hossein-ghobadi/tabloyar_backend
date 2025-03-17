using Radin.Common.Dto;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Endpoint.Site.Models.ViewModels.User
{
    public class ForgetPasswordViewModel
    {
        [Display(Name = "تلفن همراه")]
        public string phone { get; set; }

        public List<IdLabelDto> Validate()

        {
            var validationErrors = new List<IdLabelDto>();
            int id = 0;
            if (string.IsNullOrWhiteSpace(phone))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "شماره تماس خود را وارد نمایید"
                });
            }

            return validationErrors;
        }
    }

}
