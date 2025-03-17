using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentCategoryRemove;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Contents.Commands.ContentCategoryRemove.ContentCategoryRemoveService;
using static Radin.Application.Services.Ideas.Commands.IdeaCategoryRemove.IdeaCategoryRemoveService;

namespace Radin.Application.Services.Ideas.Commands.IdeaCategoryRemove
{
    public interface IIdeaCategoryRemoveService
    {
        ResultDto Execute(RequestIdeaCategoryDto request);

    }
    public class IdeaCategoryRemoveService : IIdeaCategoryRemoveService
    {
        private readonly IDataBaseContext _context;

        public IdeaCategoryRemoveService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestIdeaCategoryDto request)
        {
            var category = _context.IdeaCategories.FirstOrDefault(c => c.IdeaCategoryUniqeName == request.id);
            if (category == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "دسته بندی ایده یافت نشد"
                };
            }
            var msg = "";
            if (category.IsRemoved)
            {
                msg = "حذف  غیر فعال شد";
            }
            else
            {
                msg = "حذف  فعال شد";
            }
            category.RemoveTime = DateTime.Now;
            category.IsRemoved = !(category.IsRemoved);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = msg
            };
        }

        public class RequestIdeaCategoryDto
        {
            public string id { get; set; }
        }
    }
}
