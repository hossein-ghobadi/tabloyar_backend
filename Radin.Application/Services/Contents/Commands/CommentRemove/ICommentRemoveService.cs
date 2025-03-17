using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Contents.Commands.CommentRemove.CommentRemoveService;

namespace Radin.Application.Services.Contents.Commands.CommentRemove
{
    public interface ICommentRemoveService
    {

        ResultDto Execute(RequestCommentGetIdDto request);
    }

        public class CommentRemoveService : ICommentRemoveService
        {
            private readonly IDataBaseContext _context;

            public CommentRemoveService(IDataBaseContext context)
            {
                _context = context;
            }


            public ResultDto Execute(RequestCommentGetIdDto request)
            {

                var comment = _context.Comments.Find(request.Id);
            //var subcomment = _context.SubComments.FirstOrDefault(c => c.CommentID == comment.Id);
                var subcomment = _context.SubComments.Where(c => c.CommentID == comment.Id);
            if (comment == null)
                {
                    return new ResultDto
                    {
                        IsSuccess = false,
                        Message = "!نظر مربوطه یافت نشد"
                    };
                }
            _context.Comments.Remove(comment);
                foreach(var item in subcomment)
            {
                _context.SubComments.Remove(item);
  
            }
                _context.SaveChanges();
                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "نظر مربوطه با موفقیت حذف شد"
                };
            }

            public class RequestCommentGetIdDto
            {
                public long Id { get; set; }
            }
        }
    }

