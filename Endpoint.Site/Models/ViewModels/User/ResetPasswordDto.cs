//using Radin.Common.Dto;
//using System.ComponentModel.DataAnnotations;
//using System.Xml.Linq;

//namespace Endpoint.Site.Models.ViewModels.User
//{
//    public class ResetPasswordDto
//    {
//        [DataType(DataType.Password)]
//        [Display(Name = "پسورد")]
//        public string Password { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = " تکرار پسورد")]
//        public string ConfirmPassword { get; set;}

//        public string UserId { get; set; }
//        public string Code { get; set; }



//        public List<IdLabelDto> Validate()
//        {
//            var validationErrors = new List<IdLabelDto>();
//            int id = 0;
//            if (Password != ConfirmPassword)
//            {
//                id = id + 1;
//                validationErrors.Add(new IdLabelDto
//                {
//                    id = id,
//                    label = "!پسورد و تکرار آن باید برابر باشند"
//                });
//            }
//            return validationErrors;
//        }
//    }
//}
