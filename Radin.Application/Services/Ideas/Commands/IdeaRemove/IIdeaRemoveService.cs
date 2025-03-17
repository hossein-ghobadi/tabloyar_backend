using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentRemove;
using Radin.Common.Dto;
using Radin.Domain.Entities.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Contents.Commands.ContentRemove.ContentRemoveService;
using static Radin.Application.Services.Ideas.Commands.IIdeaRemove.IdeaRemoveService;

namespace Radin.Application.Services.Ideas.Commands.IIdeaRemove
{
    public interface IIdeaRemoveService
    {
        ResultDto Execute(RequestIdeaRemoveDto request);
        ResultDto delete (RequestIdeaRemoveDto request);

    }

    public class IdeaRemoveService : IIdeaRemoveService
    {
        private readonly IDataBaseContext _context;

        public IdeaRemoveService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestIdeaRemoveDto request)
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
            
            var msg = "";
            if (idea.IsRemoved)
            {
                msg = "منتشر شد";
            }
            else
            {
                msg = "حالت پیش نویس";
            }
            idea.RemoveTime = DateTime.Now;
            idea.IsRemoved = !(idea.IsRemoved);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = msg
            };
        }


        public ResultDto delete(RequestIdeaRemoveDto request)
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
            
            _context.Ideas.Remove(idea);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = " حذف با موفقیت انجام شد"
            };
        }

        public class RequestIdeaRemoveDto
        {
            public string id { get; set; }
        }
    }
}
