//using Microsoft.EntityFrameworkCore;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Commands.ContentSet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Contents;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Contents.Commands.ContentCategoryRemove.ContentCategoryRemoveService;

//namespace Radin.Application.Services.Contents.Commands.ContentCategoryRemove
//{
//    public interface IContentCategoryRemoveService
//    {
//        ResultDto Execute(RequestCategoryGetIdDto request);

//    }

//    public class ContentCategoryRemoveService : IContentCategoryRemoveService
//    {
//        private readonly IDataBaseContext _context;

//        public ContentCategoryRemoveService (IDataBaseContext context)
//        {
//            _context = context;
//        }


//        public ResultDto Execute(RequestCategoryGetIdDto request)
//        {
//            var category = _context.Categories.FirstOrDefault(c => c.CategoryUniqeName == request.id);
//            if (category == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "دسته بندی محتوی یافت نشد"
//                };
//            }
//            var msg = "";
//            if (category.IsRemoved)
//            {
//                msg = "حذف غیر فعال شد";
//            }
//            else
//            {
//                msg = "حذف فعال شد";
//            }
//            category.RemoveTime = DateTime.Now;
//            category.IsRemoved = !(category.IsRemoved);
//            _context.SaveChanges();
//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message = msg
//            };
//        }

//        public class RequestCategoryGetIdDto
//        {
//            public string id { get; set; }
//        }
//    }

//}
