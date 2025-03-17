using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentRemove;
using Radin.Common.Dto;
using Radin.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Commands.ContentChangeIsIndex
{
    public interface IContentIndexService
    {
        ResultDto Execute(RequestById_s request);
    }
    public class ContentIndexService : IContentIndexService
    {
        private readonly IDataBaseContext _context;

        public ContentIndexService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestById_s request)
        {

            var content = _context.Contents.FirstOrDefault(c => c.ContentUniqeName == request.id);
            if (content == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "محتوی مورد نظر یافت نشد"
                };
            }
            var msg = content.IsIndex ? "ایندکس غیر فعال شد" : "ایندکس فعال شد";
            content.IsIndex = !(content.IsIndex);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = msg
            };
        }

    }

      
}
