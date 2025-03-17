using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.ProductItems.Commands.TitleSet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Products.Aditional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.ProductItems.Commands.EdgeSizeSet
{
    public interface IEdgeSizeSetService
    {
        ResultDto<ResultEdgeSizeSetDto> Execute(RequestEdgeSizeSetDto request);

    }

    public class EdgeSizeSetService : IEdgeSizeSetService
    {
        private readonly IPriceFeeDataBaseContext _context;

        public EdgeSizeSetService(IPriceFeeDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultEdgeSizeSetDto> Execute(RequestEdgeSizeSetDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return new ResultDto<ResultEdgeSizeSetDto>()
                    {
                        Data = new ResultEdgeSizeSetDto()
                        {
                            EdgeSizeId = 0,
                        },
                        IsSuccess = false,
                        Message = "نوع تابلو را وارد نمایید"
                    };
                }
                if ((request.EdgeSize)!= null)
                {
                    return new ResultDto<ResultEdgeSizeSetDto>()
                    {
                        Data = new ResultEdgeSizeSetDto()
                        {
                            EdgeSizeId = 0,
                        },
                        IsSuccess = false,
                        Message = "اندازه لبه را وارد نمایید"
                    };
                }


                MaterialEdgeSize edgesize = new MaterialEdgeSize()
                {
                    Title = request.Title,
                    EdgeSize=request.EdgeSize

                };

                _context.MaterialEdgeSizes.Add(edgesize);

                _context.SaveChanges();

                return new ResultDto<ResultEdgeSizeSetDto>()
                {
                    Data = new ResultEdgeSizeSetDto()
                    {
                        EdgeSizeId = edgesize.Id,

                    },
                    IsSuccess = true,
                    Message = "اندازه لبه با موفقیت درج شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultEdgeSizeSetDto>()
                {
                    Data = new ResultEdgeSizeSetDto()
                    {
                        EdgeSizeId = 0,
                    },
                    IsSuccess = false,
                    Message = "اندازه لبه جدید درج نشد !"
                };
            }
        }
    }
    public class RequestEdgeSizeSetDto
    {
        public string Title { get; set; }
        public float EdgeSize { get; set; }

    }

    public class ResultEdgeSizeSetDto
    {
        public long EdgeSizeId { get; set; }

    }
}
