//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Samples.Commands.SampleCategoryRemove;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Samples.Commands.SampleCategoryRemove.SampleCategoryRemoveService;

//namespace Radin.Application.Services.Samples.Commands.SampleCategoryRemove
//{
//    public interface ISampleCategoryRemoveService
//    {
//        ResultDto Execute(RequestSampleCategoryDto request);

//    }
//    public class SampleCategoryRemoveService : ISampleCategoryRemoveService
//    {
//        private readonly IDataBaseContext _context;

//        public SampleCategoryRemoveService(IDataBaseContext context)
//        {
//            _context = context;
//        }


//        public ResultDto Execute(RequestSampleCategoryDto request)
//        {
//            var category = _context.SampleCategories.FirstOrDefault(c => c.SampleCategoryUniqeName == request.id);
//            if (category == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "دسته بندی نمونه کار یافت نشد"
//                };
//            }
//            var msg = "";
//            if(category.IsRemoved)
//            {
//                msg = "حذف غیر فعال شد";
//            }
//            else
//            {
//                msg = "حذف فعال شد" ;
//            }
//            category.RemoveTime = DateTime.Now;

//            category.IsRemoved = !(category.IsRemoved);
//            _context.SaveChanges();
//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message= msg
//            };
//        }

//        public class RequestSampleCategoryDto
//        {
//            public string id { get; set; }
//        }
//    }
//}
