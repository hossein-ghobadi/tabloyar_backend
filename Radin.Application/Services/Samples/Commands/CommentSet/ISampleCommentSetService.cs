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
//    public interface ISampleCommentSetService
//    {
//        ResultDto<ResultSampleCommentSetDto> Execute(RequestSampleCommentSetDto request);

//    }
//    public class SampleCommentSetService : ISampleCommentSetService
//    {

//        private readonly IDataBaseContext _context;

//        public SampleCommentSetService(IDataBaseContext context)
//        {
//            _context = context;

//        }
//        public ResultDto<ResultSampleCommentSetDto> Execute(RequestSampleCommentSetDto request)
//        {
//            try
//            {

//                if (string.IsNullOrEmpty(request.Name))
//                {
//                    return new ResultDto<ResultSampleCommentSetDto>()
//                    {
//                        Data = new ResultSampleCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "نام را وارد کنید"

//                    };



//                }
//                if (string.IsNullOrEmpty(request.Email))
//                {
//                    return new ResultDto<ResultSampleCommentSetDto>()
//                    {
//                        Data = new ResultSampleCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "ایمیل را وارد کنید"

//                    };
//                }

//                if (string.IsNullOrEmpty(request.UserRole))
//                {
//                    return new ResultDto<ResultSampleCommentSetDto>()
//                    {
//                        Data = new ResultSampleCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "نقش کاربر را وارد کنید"

//                    };
//                }

//                if (string.IsNullOrEmpty(request.CommentText))
//                {
//                    return new ResultDto<ResultSampleCommentSetDto>()
//                    {
//                        Data = new ResultSampleCommentSetDto()
//                        {
//                            CommentId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "متن نظر خود را وارد کنید"

//                    };



//                }

//                var sample = _context.Samples.FirstOrDefault(c => c.Id == request.SampleId);


//                SampleComment comment = new SampleComment()
//                {
//                    SampleCategoryUniqeName = sample.SampleUniqeName,
//                    SampleId = request.SampleId,
//                    SampleTitle = sample.SampleTitle,//request.ContentTitle,
//                    Name = request.Name,
//                    Email = request.Email,
//                    UserRole = request.UserRole,
//                    CommentText = request.CommentText,
//                    Situation = request.Situation,
//                };



//                _context.SampleComments.Add(comment);

//                _context.SaveChanges();

//                return new ResultDto<ResultSampleCommentSetDto>()
//                {
//                    Data = new ResultSampleCommentSetDto()
//                    {
//                        CommentId = comment.Id,
//                        UserName = request.Name,
//                        UserRole = request.UserRole,

//                    },
//                    IsSuccess = true,
//                    Message = "نظر شما با موفقیت ثبت شد",
//                };

//            }


//            catch (Exception)
//            {
//                return new ResultDto<ResultSampleCommentSetDto>()
//                {
//                    Data = new ResultSampleCommentSetDto()
//                    {
//                        CommentId = 0,
//                        UserName = request.Name,
//                        UserRole = request.UserRole,
//                    },
//                    IsSuccess = false,
//                    Message = "ثبت نظر ناموفق !"
//                };

//            }


//        }



//    }


//    public class RequestSampleCommentSetDto
//    {
//        public long SampleId { get; set; }
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string UserRole { get; set; }
//        public string CommentText { get; set; }
//        public string Situation { get; set; }
//    }

//    public class ResultSampleCommentSetDto
//    {
//        public long CommentId { get; set; }
//        public string UserName { get; set; }
//        public string UserRole { get; set; }
//    }

//}
