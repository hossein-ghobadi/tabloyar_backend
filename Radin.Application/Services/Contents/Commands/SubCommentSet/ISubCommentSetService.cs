//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Commands.CommentSet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Comments;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Design;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Contents.Commands.SubCommentSet
//{
//    public interface ISubCommentSetService
//    {
//        ResultDto<ResultSubCommentSetDto> Execute(RequestSubCommentSetDto request);
//    }


//    public class SubCommentSetService : ISubCommentSetService
//    {

//        private readonly IDataBaseContext _context;

//        public SubCommentSetService(IDataBaseContext context)
//        {
//            _context = context;

//        }
//        public ResultDto<ResultSubCommentSetDto> Execute(RequestSubCommentSetDto request)
//        {
//            try
//            {

//                if (string.IsNullOrEmpty(request.Name))
//                {
//                    return new ResultDto<ResultSubCommentSetDto>()
//                    {
//                        Data = new ResultSubCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "نام را وارد کنید"

//                    };



//                }
//                if (string.IsNullOrEmpty(request.Email))
//                {
//                    return new ResultDto<ResultSubCommentSetDto>()
//                    {
//                        Data = new ResultSubCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "ایمیل را وارد کنید"

//                    };

//                }

//                if (string.IsNullOrEmpty(request.UserRole))
//                {
//                    return new ResultDto<ResultSubCommentSetDto>()
//                    {
//                        Data = new ResultSubCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "نقش کاربر را وارد کنید"

//                    };

//                }

//                if (string.IsNullOrEmpty(request.ReplyMsg))
//                {
//                    return new ResultDto<ResultSubCommentSetDto>()
//                    {
//                        Data = new ResultSubCommentSetDto()
//                        {
//                            SubCommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "متن پاسخ خود را وارد کنید"

//                    };



//                }

//                var comment = _context.Comments.FirstOrDefault(c => c.Id == request.CommentId);


//                SubComment subcomment = new SubComment()
//                {
//                    CommentID = comment.Id,
//                    ContentId = comment.ContentId,
//                    Name = request.Name,
//                    Email = request.Email,
//                    UserRole = request.UserRole,
//                    ReplyMsg = request.ReplyMsg,
//                };



//                _context.SubComments.Add(subcomment);

//                _context.SaveChanges();

//                return new ResultDto<ResultSubCommentSetDto>()
//                {
//                    Data = new ResultSubCommentSetDto()
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
//                return new ResultDto<ResultSubCommentSetDto>()
//                {
//                    Data = new ResultSubCommentSetDto()
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


//    public class RequestSubCommentSetDto
//    {
//        public long CommentId { get; set; }
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string UserRole { get; set; }
//        public string ReplyMsg { get; set; }
//    }

//    public class ResultSubCommentSetDto
//    {
//        public long SubCommentId { get; set; }
//        public string UserName { get; set; }
//        public string UserRole { get; set; }
//        public long CommentId { get; set; }
//    }



//}
