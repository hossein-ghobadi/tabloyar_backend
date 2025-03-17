using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.HomePage.Commands.HomePageSliderEdit;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.HomePage.Commands.HomePageSliderRemove.HomePageSliderRemoveService;

namespace Radin.Application.Services.HomePage.Commands.HomePageSliderRemove
{
    public interface IHomePageSliderRemoveService
    {
        ResultDto Execute(RequestHomeSliderRemoveDto request);
    }
    


    public class HomePageSliderRemoveService : IHomePageSliderRemoveService
    {
        private readonly IDataBaseContext _context;

        public HomePageSliderRemoveService(IDataBaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(RequestHomeSliderRemoveDto request)
        {

            var Slider = _context.HomeSliders.FirstOrDefault(c => c.Id == request.id);
            if (Slider == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "محتوی مورد نظر یافت نشد"
                };
            }

            _context.HomeSliders.Remove(Slider);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "محتوی مورد نظر با موفقیت حذف شد"
            };
        }

        public class RequestHomeSliderRemoveDto
        {
            public int id { get; set; }
        }
    }
}
