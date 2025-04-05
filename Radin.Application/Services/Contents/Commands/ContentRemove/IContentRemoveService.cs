
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Commands.ContentCategoryRemove;
//using Radin.Application.Services.Contents.Queries.ContentGet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Ideas;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Contents.Commands.ContentCategoryRemove.ContentCategoryRemoveService;
//using static Radin.Application.Services.Contents.Commands.ContentRemove.ContentRemoveService;

//namespace Radin.Application.Services.Contents.Commands.ContentRemove
//{
//    public interface IContentRemoveService
//    {
//        ResultDto Execute(RequestContentGetIdDto request);
//        ResultDto delete(RequestContentGetIdDto request);
//    }

//    public class ContentRemoveService : IContentRemoveService
//    {
//        private readonly IDataBaseContext _context;

//        public ContentRemoveService(IDataBaseContext context)
//        {
//            _context = context;
//        }

//        public ResultDto Execute(RequestContentGetIdDto request)
//        {
//            try
//            {
//                // Attempt to find the content by its unique name (id)
//                var content = _context.Contents.FirstOrDefault(c => c.ContentUniqeName == request.id);

//                if (content == null)
//                {
//                    // Content not found
//                    return new ResultDto
//                    {
//                        IsSuccess = false,
//                        Message = "محتوی مورد نظر یافت نشد"  // "Content not found"
//                    };
//                }

//                var msg = content.IsRemoved ? "منتشر شد" : "حالت پیش نویس";  // Toggle message based on current status

//                // Mark content as removed/unremoved and set the removal time
//                content.RemoveTime = DateTime.Now;
//                content.IsRemoved = !content.IsRemoved;

//                // Save changes to the database
//                _context.SaveChanges();

//                return new ResultDto
//                {
//                    IsSuccess = true,
//                    Message = msg
//                };
//            }
//            catch (Exception ex)  // Catch any exceptions
//            {
//                // Log the exception if necessary and return a failure result
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = $"An error occurred: {ex.Message}"  // Provide detailed error message
//                };
//            }
//        }



//        public ResultDto delete(RequestContentGetIdDto request)
//        {
//            try
//            {
//                // Attempt to find the content by its unique name (id)
//                var content = _context.Contents.FirstOrDefault(c => c.ContentUniqeName == request.id);

//                if (content == null)
//                {
//                    // Content not found
//                    return new ResultDto
//                    {
//                        IsSuccess = false,
//                        Message = "محتوی مورد نظر یافت نشد"  // "Content not found"
//                    };
//                }

//               _context.Contents.Remove(content);
//                // Save changes to the database
//                _context.SaveChanges();

//                return new ResultDto
//                {
//                    IsSuccess = true,
//                    Message = " حذف با موفقیت انجام شد"
//                };
//            }
//            catch (Exception ex)  // Catch any exceptions
//            {
//                // Log the exception if necessary and return a failure result
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = $"An error occurred: {ex.Message}"  // Provide detailed error message
//                };
//            }
//        }








//        public class RequestContentGetIdDto
//        {
//            public string id { get; set; }  // Unique content identifier
//        }
//    }
//}
