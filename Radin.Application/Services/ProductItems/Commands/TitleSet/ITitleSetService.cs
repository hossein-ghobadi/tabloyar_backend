//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Commands.ContentCategorySet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Contents;
//using Radin.Domain.Entities.Products.Aditional;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.ProductItems.Commands.TitleSet
//{
//    public interface ITitleSetService
//    {

//        ResultDto<ResultTitleSetDto> Execute(RequestTitleSetDto request);


//    }





//    public class TitleSetService : ITitleSetService
//    {
//        private readonly IPriceFeeDataBaseContext _context;

//        public TitleSetService(IPriceFeeDataBaseContext context)
//        {
//            _context = context;
//        }
//        public ResultDto<ResultTitleSetDto> Execute(RequestTitleSetDto request)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(request.TitleName))
//                {
//                    return new ResultDto<ResultTitleSetDto>()
//                    {
//                        Data = new ResultTitleSetDto()
//                        {
//                            TitleId = 0,
//                        },
//                        IsSuccess = false,
//                        Message = "نوع تابلو را وارد نمایید"
//                    };
//                }
                

//                Title title = new Title()
//                {
//                    TitleName = request.TitleName,
                    
//                };

//                _context.Titles.Add(title);

//                _context.SaveChanges();

//                return new ResultDto<ResultTitleSetDto>()
//                {
//                    Data = new ResultTitleSetDto()
//                    {
//                        TitleId = title.Id,

//                    },
//                    IsSuccess = true,
//                    Message = "نوع تابلو با موفقیت درج شد",
//                };
//            }
//            catch (Exception)
//            {
//                return new ResultDto<ResultTitleSetDto>()
//                {
//                    Data = new ResultTitleSetDto()
//                    {
//                        TitleId = 0,
//                    },
//                    IsSuccess = false,
//                    Message = "نوع تابلو جدید درج نشد !"
//                };
//            }
//        }
//    }
//    public class RequestTitleSetDto
//    {
//        public string TitleName { get; set; }
        
//    }

//    public class ResultTitleSetDto
//    {
//        public long TitleId { get; set; }

//    }
//}
