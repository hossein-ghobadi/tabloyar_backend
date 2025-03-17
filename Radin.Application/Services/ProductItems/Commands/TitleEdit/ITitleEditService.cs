using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentCategoryEdit;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.ProductItems.Commands.TitleEdit
{
    public interface ITitleEditService
    {
        ResultDto Execute(UpdateTitleDto request);

    }

    public class TitleEditService : ITitleEditService
    {
        private readonly IPriceFeeDataBaseContext _context;

        public TitleEditService(IPriceFeeDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(UpdateTitleDto updateDto)
        {
            var title = _context.Titles.Find(updateDto.Id);
            if (title == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = " نوع تابلو یافت نشد"
                };
            }

            title.TitleName = updateDto.TitleName;
            
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش نوع تابلو انجام شد"
            };

        }
    }


    public class UpdateTitleDto
    {
        public long Id { get; set; }
        public string TitleName { get; set; }
        

    }
}
