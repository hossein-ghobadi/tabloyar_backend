using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.SubCommentSet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Comments;
using Radin.Domain.Entities.Ideas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Ideas.Commands.CommentSet
{
    public interface IIdeaSubCommentSetService
    {
        ResultDto<ResultIdeaSubCommentSetDto> Execute(RequestIdeaSubCommentSetDto request);

    }


    public class IdeaSubCommentSetService : IIdeaSubCommentSetService
    {

        private readonly IDataBaseContext _context;

        public IdeaSubCommentSetService(IDataBaseContext context)
        {
            _context = context;

        }
        public ResultDto<ResultIdeaSubCommentSetDto> Execute(RequestIdeaSubCommentSetDto request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.Name))
                {
                    return new ResultDto<ResultIdeaSubCommentSetDto>()
                    {
                        Data = new ResultIdeaSubCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام را وارد کنید"

                    };



                }
                if (string.IsNullOrEmpty(request.Email))
                {
                    return new ResultDto<ResultIdeaSubCommentSetDto>()
                    {
                        Data = new ResultIdeaSubCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "ایمیل را وارد کنید"

                    };

                }

                if (string.IsNullOrEmpty(request.UserRole))
                {
                    return new ResultDto<ResultIdeaSubCommentSetDto>()
                    {
                        Data = new ResultIdeaSubCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "نقش کاربر را وارد کنید"

                    };

                }

                if (string.IsNullOrEmpty(request.ReplyMsg))
                {
                    return new ResultDto<ResultIdeaSubCommentSetDto>()
                    {
                        Data = new ResultIdeaSubCommentSetDto()
                        {
                            SubCommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "متن پاسخ خود را وارد کنید"

                    };



                }

                var comment = _context.IdeaComments.FirstOrDefault(c => c.Id == request.CommentId);


                IdeaSubComment subcomment = new IdeaSubComment()
                {
                    CommentID = comment.Id,
                    IdeaId = comment.IdeaId,
                    Name = request.Name,
                    Email = request.Email,
                    UserRole = request.UserRole,
                    ReplyMsg = request.ReplyMsg,
                };



                _context.IdeaSubComments.Add(subcomment);

                _context.SaveChanges();

                return new ResultDto<ResultIdeaSubCommentSetDto>()
                {
                    Data = new ResultIdeaSubCommentSetDto()
                    {
                        SubCommentId = subcomment.Id,
                        UserName = request.Name,
                        UserRole = request.UserRole,
                        CommentId = request.CommentId,

                    },
                    IsSuccess = true,
                    Message = "پاسخ شما با موفقیت ثبت شد",
                };

            }


            catch (Exception)
            {
                return new ResultDto<ResultIdeaSubCommentSetDto>()
                {
                    Data = new ResultIdeaSubCommentSetDto()
                    {
                        SubCommentId = 0,
                        UserName = request.Name,
                        UserRole = request.UserRole,
                        CommentId = request.CommentId,
                    },
                    IsSuccess = false,
                    Message = "ثبت پاسخ ناموفق !"
                };

            }


        }



    }


    public class RequestIdeaSubCommentSetDto
    {
        public long CommentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string ReplyMsg { get; set; }
    }

    public class ResultIdeaSubCommentSetDto
    {
        public long SubCommentId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public long CommentId { get; set; }
    }

}
