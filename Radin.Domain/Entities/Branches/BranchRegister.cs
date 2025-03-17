using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Branches
{
    
        public class BranchRegister
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [Required(ErrorMessage = "نام الزامی است")]
            [MaxLength(50, ErrorMessage = "نام نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string fName { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [Required(ErrorMessage = "نام خانوادگی الزامی است")]
            [MaxLength(50, ErrorMessage = "نام خانوادگی نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string lName { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [Required(ErrorMessage = "شهر محل سکونت الزامی است")]
            [MaxLength(50, ErrorMessage = "شهر محل سکونت نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string city { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [Required(ErrorMessage = "سن الزامی است")]
            [Range(18, 99, ErrorMessage = "سن باید بین ۱۸ تا ۹۹ سال باشد")]
            public string age { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [Required(ErrorMessage = "تلفن همراه الزامی است")]
            [Phone(ErrorMessage = "شماره تلفن معتبر نیست")]
            public string phone { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [MaxLength(50, ErrorMessage = "شغل فعلی نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string occupation { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [MaxLength(50, ErrorMessage = "سابقه فعالیت نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string yearsOfService { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [MaxLength(50, ErrorMessage = "شهر محل فعالیت نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string cityOfService { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [MaxLength(50, ErrorMessage = "شهر مد نظر نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string desiredCity { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [MaxLength(50, ErrorMessage = "نام مجموعه فعلی نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string currentCompany { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [MaxLength(50, ErrorMessage = "متوسط درآمد ماهیانه فعلی نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string avgCurrSalary { get; set; }

            [Column(TypeName = "nvarchar(50)")]
            [MaxLength(50, ErrorMessage = "نوع همکاری نمی‌تواند بیش از ۵۰ کاراکتر باشد")]
            public string Btype { get; set; }

            [MaxLength(500, ErrorMessage = "توضیحات بیشتر نمی‌تواند بیش از ۵۰۰ کاراکتر باشد")]
            public string aboutYou { get; set; }

            public bool IsRead { get; set; } = false;
        }
    
}
