using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentChangeIsIndex;
using Radin.Common.Dto;
using Radin.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Ideas.Commands.IdeaIndex
{
    public interface IIdeaIndexService
    {
        ResultDto Execute(RequestById_s request);
    }
    public class IdeaIndexService : IIdeaIndexService
    {
        private readonly IDataBaseContext _context;

        public IdeaIndexService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestById_s request)
        {

            var idea = _context.Ideas.FirstOrDefault(c => c.IdeaUniqeName == request.id);
            if (idea == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "ایده مورد نظر یافت نشد"
                };
            }
            var msg = idea.IsIndex ? "ایندکس غیر فعال شد" : "ایندکس فعال شد";
            idea.IsIndex = !(idea.IsIndex);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = msg
            };
        }

    }
}
