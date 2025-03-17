using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.CommentSet;
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
    public interface IIdeaCommentSetService
    {
        ResultDto<ResultIdeaCommentSetDto> Execute(RequestIdeaCommentSetDto request);

    }



    public class IdeaCommentSetService : IIdeaCommentSetService
    {

        private readonly IDataBaseContext _context;

        public IdeaCommentSetService(IDataBaseContext context)
        {
            _context = context;

        }
        public ResultDto<ResultIdeaCommentSetDto> Execute(RequestIdeaCommentSetDto request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.Name))
                {
                    return new ResultDto<ResultIdeaCommentSetDto>()
                    {
                        Data = new ResultIdeaCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام را وارد کنید"

                    };



                }
                if (string.IsNullOrEmpty(request.Email))
                {
                    return new ResultDto<ResultIdeaCommentSetDto>()
                    {
                        Data = new ResultIdeaCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "ایمیل را وارد کنید"

                    };
                }

                if (string.IsNullOrEmpty(request.UserRole))
                {
                    return new ResultDto<ResultIdeaCommentSetDto>()
                    {
                        Data = new ResultIdeaCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "نقش کاربر را وارد کنید"

                    };
                }

                if (string.IsNullOrEmpty(request.CommentText))
                {
                    return new ResultDto<ResultIdeaCommentSetDto>()
                    {
                        Data = new ResultIdeaCommentSetDto()
                        {
                            CommentId = 0,
                        },
                        IsSuccess = false,
                        Message = "متن نظر خود را وارد کنید"

                    };



                }

                var idea = _context.Ideas.FirstOrDefault(c => c.Id == request.IdeaId);


                IdeaComment comment = new IdeaComment()
                {
                    IdeaCategoryUniqeName = idea.IdeaUniqeName,
                    IdeaId = request.IdeaId,
                    IdeaTitle = idea.IdeaTitle,//request.ContentTitle,
                    Name = request.Name,
                    Email = request.Email,
                    UserRole = request.UserRole,
                    CommentText = request.CommentText,
                    Situation = request.Situation,
                };



                _context.IdeaComments.Add(comment);

                _context.SaveChanges();

                return new ResultDto<ResultIdeaCommentSetDto>()
                {
                    Data = new ResultIdeaCommentSetDto()
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
                return new ResultDto<ResultIdeaCommentSetDto>()
                {
                    Data = new ResultIdeaCommentSetDto()
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


    public class RequestIdeaCommentSetDto
    {
        public long IdeaId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string CommentText { get; set; }
        public string Situation { get; set; }
    }

    public class ResultIdeaCommentSetDto
    {
        public long CommentId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }

}
