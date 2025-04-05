//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Commands.ContentCategoryRemove;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.ProductItems.Commands.TitleRemove
//{
//    public interface ITitleRemoveService
//    {
//        ResultDto Execute(long titleId);

//    }
//    public class TitleRemoveService : ITitleRemoveService
//    {
//        private readonly IPriceFeeDataBaseContext _context;

//        public TitleRemoveService(IPriceFeeDataBaseContext context)
//        {
//            _context = context;
//        }


//        public ResultDto Execute(long TitleId)
//        {

//            var title = _context.Titles.Find(TitleId);
//            if (title == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "نوع تابلو مورد نظر یافت نشد"
//                };
//            }
//            //title.RemoveTime = DateTime.Now;
//            //title.IsRemoved = true;
//            _context.SaveChanges();
//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message = "نوع تابلو مورد نظر با موفقیت حذف شد"
//            };
//        }
//    }
//}
