using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentEdit;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using Radin.Domain.Entities.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.HomePage.Commands.HomePageSliderEdit
{
    public interface IHomePageSliderEditService
    {
        ResultDto<ResultHomeSliderEditDto> Execute(RequestHomeSliderEditDto request);
    }
    

    public class HomePageSliderEditService : IHomePageSliderEditService
    {

        private readonly IDataBaseContext _context;

        public HomePageSliderEditService(IDataBaseContext context)
        {
            _context = context;


        }
        public ResultDto<ResultHomeSliderEditDto> Execute(RequestHomeSliderEditDto request)
        {

            var Errors = new List<IdLabelDto>();
            try
            {
                var Slider = _context.HomeSliders.FirstOrDefault(c => c.Id == request.Id);
                if (Slider == null)
                {
                    return new ResultDto<ResultHomeSliderEditDto>()
                    {
                        Data = new ResultHomeSliderEditDto()
                        {
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "!محتوی مورد نظر یافت نشد"
                    };
                }

                int id = 0;

                //if (string.IsNullOrWhiteSpace(request.Title))
                //{
                //    id = id + 1;
                //    Errors.Add(new IdLabelDto
                //    {
                //        id = id,
                //        label = "!عنوان عکس را وارد نمایید"
                //    });
                //}



                if (string.IsNullOrWhiteSpace(request.Image))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عکس راآپلود نمایید"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.Sorting.ToString()))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی را وارد نمایید"
                    });
                }

                if (request.Sorting.GetType() != typeof(int))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
                    });
                }
                if (request.Sorting is int & request.Sorting < 1)
                {

                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
                    });
                }
                //if (string.IsNullOrWhiteSpace(request.Description))
                //{
                //    id = id + 1;
                //    Errors.Add(new IdLabelDto
                //    {
                //        id = id,
                //        label = "!توضیحات عکس را وارد نمایید"
                //    });
                //}

                if (Errors.Count() < 1)
                {



                    Slider.Title = request.Title;
                    Slider.Image = request.Image;
                    Slider.Description = request.Description;
                    Slider.Sorting = request.Sorting;
                    

                    _context.SaveChanges();

                    return new ResultDto<ResultHomeSliderEditDto>()
                    {
                        Data = new ResultHomeSliderEditDto()
                        {
                            SliderId = Slider.Id,
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = " !ویرایش محتوی با موفقیت انجام شد",
                    };
                }

                else
                {
                    return new ResultDto<ResultHomeSliderEditDto>()
                    {
                        Data = new ResultHomeSliderEditDto()
                        {
                            SliderId = 0,
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "!محتوی اسلایدر ویرایش نشد"
                    };

                }

            }

            catch (Exception)
            {
                return new ResultDto<ResultHomeSliderEditDto>()
                {
                    Data = new ResultHomeSliderEditDto()
                    {
                        SliderId = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "!محتوی اسلایدر ویرایش نشد"
                };

            }
        }
    }


    public class RequestHomeSliderEditDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Sorting { get; set; }
        public string? base64 { get; set; } = "";
    }
    public class ResultHomeSliderEditDto
    {
        public long SliderId { get; set; }
        public List<IdLabelDto> Errors { get; set; }
    }
}
