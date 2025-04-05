//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Commands.ContentSet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Contents;
//using Radin.Domain.Entities.HomePage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.HomePage.Commands.HomePageSliderSet
//{
//    public interface IHomePageSliderSetService
//    {
//        ResultDto<ResultHomeSliderSetDto> Execute(RequestHomeSliderSetDto request);

//    }

//    public class HomePageSliderSetService : IHomePageSliderSetService
//    {

//        private readonly IDataBaseContext _context;

//        public HomePageSliderSetService(IDataBaseContext context)
//        {
//            _context = context;


//        }
//        public ResultDto<ResultHomeSliderSetDto> Execute(RequestHomeSliderSetDto request)
//        {

//            var Errors = new List<IdLabelDto>();
//            try
//            {

//                int id = 0;
               
//                //if (string.IsNullOrWhiteSpace(request.Title))
//                //{
//                //    id = id + 1;
//                //    Errors.Add(new IdLabelDto
//                //    {
//                //        id = id,
//                //        label = "!عنوان عکس را وارد نمایید"
//                //    });
//                //}
               


//                if (string.IsNullOrWhiteSpace(request.Image))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عکس راآپلود نمایید"
//                    });
//                }

//                //if (string.IsNullOrWhiteSpace(request.Description))
//                //{
//                //    id = id + 1;
//                //    Errors.Add(new IdLabelDto
//                //    {
//                //        id = id,
//                //        label = "!توضیحات عکس را وارد نمایید"
//                //    });
//                //}

//                //if (string.IsNullOrWhiteSpace(request.Description))
//                //{
//                //    id = id + 1;
//                //    Errors.Add(new IdLabelDto
//                //    {
//                //        id = id,
//                //        label = "!توضیحات عکس را وارد نمایید"
//                //    });
//                //}
//                if (string.IsNullOrWhiteSpace(request.Sorting.ToString()))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی را وارد نمایید"
//                    });
//                }

//                if (request.Sorting.GetType() != typeof(int))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
//                    });
//                }
//                if (request.Sorting is int & request.Sorting < 1)
//                {

//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
//                    });
//                }
//                if (Errors.Count() < 1)
//                {

//                    HomeSlider SliderContent = new HomeSlider()
//                    {
//                        Title= request.Title,
//                        Image= request.Image,
//                        Description= request.Description,
//                        Sorting=request.Sorting,
//                        Base64=request.base64
//                    };

//                    _context.HomeSliders.Add(SliderContent);
//                    _context.SaveChanges();

//                    return new ResultDto<ResultHomeSliderSetDto>()
//                    {
//                        Data = new ResultHomeSliderSetDto()
//                        {
//                            SliderId = SliderContent.Id,
//                            Errors = Errors,
//                        },
//                        IsSuccess = true,
//                        Message = " محتوی اسلایدر جدید با موفقیت درج شد",
//                    };
//                }

//                else
//                {
//                    return new ResultDto<ResultHomeSliderSetDto>()
//                    {
//                        Data = new ResultHomeSliderSetDto()
//                        {
//                            SliderId = 0,
//                            Errors = Errors,
//                        },
//                        IsSuccess = false,
//                        Message = "!محتوی اسلایدر جدید درج نشد"
//                    };

//                }

//            }

//            catch (Exception)
//            {
//                return new ResultDto<ResultHomeSliderSetDto>()
//                {
//                    Data = new ResultHomeSliderSetDto()
//                    {
//                        SliderId = 0,
//                        Errors = Errors,
//                    },
//                    IsSuccess = false,
//                    Message = "!محتوی اسلایدر جدید درج نشد"
//                };

//            }
//        }
//    }


//    public class RequestHomeSliderSetDto
//    {
//        public string Title { get; set; }       
//        public string Description { get; set; }
//        public string Image {  get; set; }
//        public int Sorting { get; set; }
//        public string base64 { get; set; } = "";
//    }
//    public class ResultHomeSliderSetDto
//    {
//        public long SliderId { get; set; }
//        public List<IdLabelDto> Errors { get; set; }
//    }
//}
