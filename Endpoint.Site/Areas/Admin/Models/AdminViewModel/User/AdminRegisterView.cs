using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Radin.Common.Dto;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace Endpoint.Site.Areas.Admin.Models.AdminViewModel.User
{
    public class AdminRegisterView
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

        [Display(Name = "نقش کاربر ")]
        public List<string> UserRole { get; set; }

        [Display(Name = "کد شعبه ")]
        public int BranchCode { get; set; }

        public List<IdLabelDto> Validate()
        {
            var validationErrors = new List<IdLabelDto>();
            int id = 0;
            if (string.IsNullOrWhiteSpace(email))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "ایمیل را وارد نمایید"
                });
            }
            if (string.IsNullOrWhiteSpace(name))
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
            if (UserRole.Count() < 1)
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "نقش کاربر را انتخاب نمایید"
                });
            }

            if (password != rePassword)
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!پسورد و تکرار آن باید برابر باشند"
                });
            }


            // Additional validation for other properties...

            return validationErrors;
        }
    }

}
