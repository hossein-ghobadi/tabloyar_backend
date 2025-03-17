using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Branch.Commands.BranchRegisterService
{
    public class BranchRegisterModel
    {


        //[Column(TypeName = "varchar(50)")]

        public string fName { get; set; }

        //[Column(TypeName = "varchar(50)")]

        public string lName { get; set; }

        //[Column(TypeName = "varchar(50)")]

        public string city { get; set; }

        //[Column(TypeName = "varchar(50)")]

        public int age { get; set; }

        //[Column(TypeName = "varchar(50)")]

        public string phone { get; set; }

        //[Column(TypeName = "varchar(50)")]

        public string occupation { get; set; }

        //[Column(TypeName = "varchar(50)")]
        public string yearsOfService { get; set; }

        //[Column(TypeName = "varchar(50)")]
        public string cityOfService { get; set; }

        //[Column(TypeName = "varchar(50)")]
        public string desiredCity { get; set; }

        //[Column(TypeName = "varchar(50)")]
        public string currentCompany { get; set; }

        //[Column(TypeName = "varchar(50)")]
        public string avgCurrSalary { get; set; }

        //[Column(TypeName = "varchar(50)")]
        public string Btype { get; set; }

        public string aboutYou { get; set; }
        public string? verifyCode { get; set; }


        public List<IdLabelDto> Validate()
        {
            var validationErrors = new List<IdLabelDto>();
            int id = 0;

            if (string.IsNullOrWhiteSpace(fName))
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "نام الزامی است" });
            }
            if (string.IsNullOrWhiteSpace(lName))
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "نام خانوادگی الزامی است" });
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "شهر محل سکونت الزامی است" });
            }
            if (age==null  || age < 18 || age > 99)
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "سن باید بین ۱۸ تا ۹۹ سال باشد" });
            }
            if (string.IsNullOrWhiteSpace(phone) || !System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{10,15}$"))
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "شماره تلفن معتبر نیست" });
            }

            // Validate occupation
            if (!string.IsNullOrWhiteSpace(occupation) && occupation.Length > 50)
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "شغل فعلی نمی‌تواند بیش از ۵۰ کاراکتر باشد" });
            }

            // Validate yearsOfService
            if (!string.IsNullOrWhiteSpace(yearsOfService) && yearsOfService.Length > 50)
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "سابقه فعالیت نمی‌تواند بیش از ۵۰ کاراکتر باشد" });
            }

            // Validate cityOfService
            if (!string.IsNullOrWhiteSpace(cityOfService) && cityOfService.Length > 50)
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "شهر محل فعالیت نمی‌تواند بیش از ۵۰ کاراکتر باشد" });
            }

            // Validate desiredCity
            if (!string.IsNullOrWhiteSpace(desiredCity) && desiredCity.Length > 50)
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "شهر مد نظر نمی‌تواند بیش از ۵۰ کاراکتر باشد" });
            }

            // Validate currentCompany
            if (!string.IsNullOrWhiteSpace(currentCompany) && currentCompany.Length > 50)
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "نام مجموعه فعلی نمی‌تواند بیش از ۵۰ کاراکتر باشد" });
            }

            // Validate avgCurrSalary
            if (!string.IsNullOrWhiteSpace(avgCurrSalary) && avgCurrSalary.Length > 50)
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "متوسط درآمد ماهیانه فعلی نمی‌تواند بیش از ۵۰ کاراکتر باشد" });
            }

            // Validate Btype
            if (!string.IsNullOrWhiteSpace(Btype) && Btype.Length > 50)
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "نوع همکاری نمی‌تواند بیش از ۵۰ کاراکتر باشد" });
            }

            // Validate aboutYou
            if (!string.IsNullOrWhiteSpace(aboutYou) && aboutYou.Length > 500)
            {
                id++;
                validationErrors.Add(new IdLabelDto { id = id, label = "توضیحات بیشتر نمی‌تواند بیش از ۵۰۰ کاراکتر باشد" });
            }

            return validationErrors;
        }
    }
    public class Phone
    {
        public string phoneNumber { get; set; } 
    }

}
