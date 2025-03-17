using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.CommentRemove;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Radin.Application.Services.Ideas.Commands.CommentRemove.IdeaCommentRemoveService;

namespace Radin.Application.Services.Ideas.Commands.CommentRemove
{
    public interface IIdeaCommentRemoveService
    {
        ResultDto Execute(RequestIdeaCommentGetIdDto request);

    }


    public class IdeaCommentRemoveService : IIdeaCommentRemoveService
    {
        private readonly IDataBaseContext _context;

        public IdeaCommentRemoveService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestIdeaCommentGetIdDto request)
        {

            var comment = _context.IdeaComments.Find(request.Id);
            //var subcomment = _context.SubComments.FirstOrDefault(c => c.CommentID == comment.Id);
            var subcomment = _context.IdeaSubComments.Where(c => c.CommentID == comment.Id);
            if (comment == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "!نظر مربوطه یافت نشد"
                };
            }
            _context.IdeaComments.Remove(comment);
            foreach (var item in subcomment)
            {
                _context.IdeaSubComments.Remove(item);

            }
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "نظر مربوطه با موفقیت حذف شد"
            };
        }

        public class RequestIdeaCommentGetIdDto
        {
            public long Id { get; set; }
        }
    }
}
