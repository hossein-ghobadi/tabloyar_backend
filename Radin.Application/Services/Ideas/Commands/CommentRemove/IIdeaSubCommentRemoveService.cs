using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.CommentRemove;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Ideas.Commands.CommentRemove.IdeaSubCommentRemoveService;

namespace Radin.Application.Services.Ideas.Commands.CommentRemove
{
    public interface IIdeaSubCommentRemoveService
    {
        ResultDto Execute(RequestIdeaSubCommentId request);

    }


    public class IdeaSubCommentRemoveService : IIdeaSubCommentRemoveService
    {
        private readonly IDataBaseContext _context;

        public IdeaSubCommentRemoveService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestIdeaSubCommentId request)
        {

            var subcomment = _context.IdeaSubComments.Find(request.Id);
            if (subcomment == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "!پاسخ مربوطه یافت نشد"
                };
            }
            _context.IdeaSubComments.Remove(subcomment);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "پاسخ مربوطه با موفقیت حذف شد"
            };
        }

        public class RequestIdeaSubCommentId
        {
            public long Id { get; set; }
        }
    }
}
