using Radin.Common.Dto;
using System.ComponentModel.DataAnnotations;

namespace Endpoint.Site.Models.ViewModels.User
{
    public class EditViewModel
    {

        [Display(Name = "نام کاربری")]
        public string username { get; set; }

        [Display(Name = "نام کامل")]
        public string fullName { get; set; }

        [Display(Name = "تلفن همراه")]
        public string phone { get; set; }

        public string? phone2 { get; set; }
        public string? gender { get; set; }
        public string? job { get; set; }
        public int? state { get; set; }
        public int? city { get; set; }
        public DateTime? age { get; set; }
        public string? address { get; set; }

        public List<IdLabelDto> Validate()
        {
            var validationErrors = new List<IdLabelDto>();
            int id = 0;
            if (string.IsNullOrWhiteSpace(username))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "نام کاربری را وارد نمایید"
                });
            }
            if (string.IsNullOrWhiteSpace(fullName))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "نام و نام خانوادگی را وارد نمایید"
                });
            }

            // Additional validation for other properties...

            return validationErrors;
        }
    }
}
