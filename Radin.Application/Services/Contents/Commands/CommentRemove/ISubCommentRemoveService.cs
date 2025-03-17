using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Contents.Commands.CommentRemove.SubCommentRemoveService;

namespace Radin.Application.Services.Contents.Commands.CommentRemove
{
    public interface ISubCommentRemoveService
    {
        ResultDto Execute(RequestSubCommentId request);
    }

    public class SubCommentRemoveService : ISubCommentRemoveService
    {
        private readonly IDataBaseContext _context;

        public SubCommentRemoveService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestSubCommentId request)
        {

            var subcomment = _context.SubComments.Find(request.Id);
            if (subcomment == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "!پاسخ مربوطه یافت نشد"
                };
            }
            _context.SubComments.Remove(subcomment);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "پاسخ مربوطه با موفقیت حذف شد"
            };
        }

        public class RequestSubCommentId
        {
            public long Id { get; set; }
        }
    }




}
