//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Samples.Commands.CommentRemove;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Samples.Commands.CommentRemove.SampleSubCommentRemoveService;

//namespace Radin.Application.Services.Samples.Commands.CommentRemove
//{
//    public interface ISampleSubCommentRemoveService
//    {
//        ResultDto Execute(RequestSampleSubCommentId request);

//    }


//    public class SampleSubCommentRemoveService : ISampleSubCommentRemoveService
//    {
//        private readonly IDataBaseContext _context;

//        public SampleSubCommentRemoveService(IDataBaseContext context)
//        {
//            _context = context;
//        }


//        public ResultDto Execute(RequestSampleSubCommentId request)
//        {

//            var subcomment = _context.SampleSubComments.Find(request.Id);
//            if (subcomment == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "!پاسخ مربوطه یافت نشد"
//                };
//            }
//            _context.SampleSubComments.Remove(subcomment);
//            _context.SaveChanges();
//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message = "پاسخ مربوطه با موفقیت حذف شد"
//            };
//        }

//        public class RequestSampleSubCommentId
//        {
//            public long Id { get; set; }
//        }
//    }
//}
