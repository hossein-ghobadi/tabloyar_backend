//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Samples.Commands.CommentSet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Samples;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Samples.Commands.CommentSet
//{
//    public interface ISampleSubCommentSetService
//    {
//        ResultDto<ResultSampleSubCommentSetDto> Execute(RequestSampleSubCommentSetDto request);

//    }

//    public class SampleSubCommentSetService : ISampleSubCommentSetService
//    {

//        private readonly IDataBaseContext _context;

//        public SampleSubCommentSetService(IDataBaseContext context)
//        {
//            _context = context;

//        }
//        public ResultDto<ResultSampleSubCommentSetDto> Execute(RequestSampleSubCommentSetDto request)
//        {
//            try
//            {

//                if (string.IsNullOrEmpty(request.Name))
//                {
//                    return new ResultDto<ResultSampleSubCommentSetDto>()
//                    {
//                        Data = new ResultSampleSubCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "نام را وارد کنید"

//                    };



//                }
//                if (string.IsNullOrEmpty(request.Email))
//                {
//                    return new ResultDto<ResultSampleSubCommentSetDto>()
//                    {
//                        Data = new ResultSampleSubCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "ایمیل را وارد کنید"

//                    };

//                }

//                if (string.IsNullOrEmpty(request.UserRole))
//                {
//                    return new ResultDto<ResultSampleSubCommentSetDto>()
//                    {
//                        Data = new ResultSampleSubCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "نقش کاربر را وارد کنید"

//                    };

//                }

//                if (string.IsNullOrEmpty(request.ReplyMsg))
//                {
//                    return new ResultDto<ResultSampleSubCommentSetDto>()
//                    {
//                        Data = new ResultSampleSubCommentSetDto()
//                        {
//                            SubCommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "متن پاسخ خود را وارد کنید"

//                    };



//                }

//                var comment = _context.SampleComments.FirstOrDefault(c => c.Id == request.CommentId);


//                SampleSubComment subcomment = new SampleSubComment()
//                {
//                    CommentID = comment.Id,
//                    SampleId = comment.SampleId,
//                    Name = request.Name,
//                    Email = request.Email,
//                    UserRole = request.UserRole,
//                    ReplyMsg = request.ReplyMsg,
//                };



//                _context.SampleSubComments.Add(subcomment);

//                _context.SaveChanges();

//                return new ResultDto<ResultSampleSubCommentSetDto>()
//                {
//                    Data = new ResultSampleSubCommentSetDto()
//                    {
//                        SubCommentId = subcomment.Id,
//                        UserName = request.Name,
//                        UserRole = request.UserRole,
//                        CommentId = request.CommentId,

//                    },
//                    IsSuccess = true,
//                    Message = "پاسخ شما با موفقیت ثبت شد",
//                };

//            }


//            catch (Exception)
//            {
//                return new ResultDto<ResultSampleSubCommentSetDto>()
//                {
//                    Data = new ResultSampleSubCommentSetDto()
//                    {
//                        SubCommentId = 0,
//                        UserName = request.Name,
//                        UserRole = request.UserRole,
//                        CommentId = request.CommentId,
//                    },
//                    IsSuccess = false,
//                    Message = "ثبت پاسخ ناموفق !"
//                };

//            }


//        }



//    }


//    public class RequestSampleSubCommentSetDto
//    {
//        public long CommentId { get; set; }
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string UserRole { get; set; }
//        public string ReplyMsg { get; set; }
//    }

//    public class ResultSampleSubCommentSetDto
//    {
//        public long SubCommentId { get; set; }
//        public string UserName { get; set; }
//        public string UserRole { get; set; }
//        public long CommentId { get; set; }
//    }
//}
