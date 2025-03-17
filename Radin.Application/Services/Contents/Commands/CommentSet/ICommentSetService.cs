using Microsoft.VisualBasic;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentSet;
using Radin.Application.Services.Contents.Queries.ContentGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Comments;
using Radin.Domain.Entities.Contents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Commands.CommentSet
{
    public interface ICommentSetService
    {
        ResultDto<ResultCommentSetDto> Execute(RequestCommentSetDto request);
    }


    public class CommentSetService : ICommentSetService
    {

        private readonly IDataBaseContext _context;

        public CommentSetService(IDataBaseContext context)
        {
            _context = context;

        }
        public ResultDto<ResultCommentSetDto> Execute(RequestCommentSetDto request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.Name))
                {
                    return new ResultDto<ResultCommentSetDto>()
                    {
                        Data = new ResultCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام را وارد کنید"

                    };



                }
                if (string.IsNullOrEmpty(request.Email))
                {
                    return new ResultDto<ResultCommentSetDto>()
                    {
                        Data = new ResultCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "ایمیل را وارد کنید"

                    };
                }

                if (string.IsNullOrEmpty(request.UserRole))
                {
                    return new ResultDto<ResultCommentSetDto>()
                    {
                        Data = new ResultCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "نقش کاربر را وارد کنید"

                    };
                }

                if (string.IsNullOrEmpty(request.CommentText))
                {
                    return new ResultDto<ResultCommentSetDto>()
                    {
                        Data = new ResultCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "متن نظر خود را وارد کنید"

                    };



                }

                var content = _context.Contents.FirstOrDefault(c => c.Id == request.ContentId);


                Comment comment = new Comment()
                {
                    CategoryUniqeName = content.CategoryUniqeName,
                    ContentId = request.ContentId,
                    ContentTitle = content.ContentTitle,//request.ContentTitle,
                    Name = request.Name,
                    Email = request.Email,
                    UserRole = request.UserRole,
                    CommentText = request.CommentText,
                    Situation = request.Situation,
                };



                _context.Comments.Add(comment);

                _context.SaveChanges();

                return new ResultDto<ResultCommentSetDto>()
                {
                    Data = new ResultCommentSetDto()
                    {
                        CommentId = comment.Id,
                        UserName = request.Name,
                        UserRole = request.UserRole,

                    },
                    IsSuccess = true,
                    Message = "نظر شما با موفقیت ثبت شد",
                };

            }


            catch (Exception)
            {
                return new ResultDto<ResultCommentSetDto>()
                {
                    Data = new ResultCommentSetDto()
                    {
                        CommentId = 0,
                        UserName = request.Name,
                        UserRole = request.UserRole,
                    },
                    IsSuccess = false,
                    Message = "ثبت نظر ناموفق !"
                };

            }


        }



    }


    public class RequestCommentSetDto
    {
        public long ContentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string CommentText { get; set; }
        public string Situation { get; set; }
    }

    public class ResultCommentSetDto
    {
        public long CommentId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }

    
}
