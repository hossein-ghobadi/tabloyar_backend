//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Samples.Commands.CommentRemove;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Samples.Commands.CommentRemove.SampleCommentRemoveService;

//namespace Radin.Application.Services.Samples.Commands.CommentRemove
//{
//    public interface ISampleCommentRemoveService
//    {
//        ResultDto Execute(RequestSampleCommentGetIdDto request);

//    }

//    public class SampleCommentRemoveService : ISampleCommentRemoveService
//    {
//        private readonly IDataBaseContext _context;

//        public SampleCommentRemoveService(IDataBaseContext context)
//        {
//            _context = context;
//        }


//        public ResultDto Execute(RequestSampleCommentGetIdDto request)
//        {

//            var comment = _context.SampleComments.Find(request.Id);
//            //var subcomment = _context.SubComments.FirstOrDefault(c => c.CommentID == comment.Id);
//            var subcomment = _context.SampleSubComments.Where(c => c.CommentID == comment.Id);
//            if (comment == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "!نظر مربوطه یافت نشد"
//                };
//            }
//            _context.SampleComments.Remove(comment);
//            foreach (var item in subcomment)
//            {
//                _context.SampleSubComments.Remove(item);

//            }
//            _context.SaveChanges();
//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message = "نظر مربوطه با موفقیت حذف شد"
//            };
//        }

//        public class RequestSampleCommentGetIdDto
//        {
//            public long Id { get; set; }
//        }
//    }
//}
