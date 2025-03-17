using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.HomePage.Queries.HomePageSlider.HomePageSliderGet;

namespace Radin.Application.Services.HomePage.Queries.HomePageSlider
{
    public interface IHomePageSliderGet
    {
        ResultDto<List<SliderContent>> MainPageGet();
        ResultDto<List<abstractedSliderContent>> AdminPageGet();
        ResultDto<SliderContent> SingleSliderGet(int? id);
    }

    public class HomePageSliderGet : IHomePageSliderGet
    {
        private readonly IDataBaseContext _context;
        public HomePageSliderGet(IDataBaseContext Context)
        {
            _context = Context;

        }
        public ResultDto<List<abstractedSliderContent>> AdminPageGet()
        {
            var SliderData = _context.HomeSliders.Select(p => new abstractedSliderContent
            {
                id = p.Id,
                Title = p.Title,
                Sorting = p.Sorting,
                
            }).ToList();
            if (SliderData.Any())
            {

                return new ResultDto<List<abstractedSliderContent>>()
                {

                    Data = SliderData,
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };
            }
            else
            {
                return new ResultDto<List<abstractedSliderContent>>()
                {

                    Data = SliderData,
                    IsSuccess = false,
                    Message = "دریافت ناموفق"

                };
            }

        }

        public ResultDto<List<SliderContent>> MainPageGet()
        {
            var SliderData = _context.HomeSliders.Select(p => new SliderContent
            {
                id = p.Id,
                Title = "",//p.Title
                Sorting = p.Sorting,
                Image = p.Image,
                Description = "",//p.Description
                Base64=p.Base64

            }).ToList();
            if (SliderData.Any())
            {

                return new ResultDto<List<SliderContent>>()
                {

                    Data = SliderData,
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };
            }
            else
            {
                return new ResultDto<List<SliderContent>>()
                {

                    Data = SliderData,
                    IsSuccess = false,
                    Message = "دریافت ناموفق"

                };
            }

        }

        public ResultDto<SliderContent> SingleSliderGet(int? id)
        {
            
                var SliderData = _context.HomeSliders.Where(p => p.Id == id).Select(p => new SliderContent
                {
                    id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Image = p.Image,
                    Sorting = p.Sorting,
                }).FirstOrDefault();
            
            if (SliderData!=null)
            {

                return new ResultDto<SliderContent>()
                {

                    Data = SliderData,
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };
            }
            else
            {
                return new ResultDto<SliderContent>()
                {

                    Data = SliderData,
                    IsSuccess = false,
                    Message = "دریافت ناموفق"

                };
            }
        }




        public class abstractedSliderContent
        {
            public int id { get; set; }
            public string Title { get; set; }
            public int Sorting { get; set; }
        }

        public class SliderContent
        {
            public int id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
            public int Sorting { get; set; }
            public string Base64 { get; set; } = "";
        }

    }
}
