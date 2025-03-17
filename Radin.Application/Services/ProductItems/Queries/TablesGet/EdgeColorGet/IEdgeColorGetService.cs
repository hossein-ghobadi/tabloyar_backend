using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.ProductItems.Queries.TitleGet;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.ProductItems.Queries.TablesGet.EdgeColorGet
{
    public interface IEdgeColorGetService
    {
        ResultDto<List<GetColorDto>> Execute();

    }

    public class EdgeColorGetService : IEdgeColorGetService
    {
        private readonly IPriceFeeDataBaseContext _context;
        public EdgeColorGetService(IPriceFeeDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<List<GetColorDto>> Execute()
        {
            var Colors = _context.MaterialEdgeColors;

            //int rowsCount = 0;
            var ColorsList = Colors.Select(p => new GetColorDto
            {
                id = p.Id,
                label = p.EdgeColor,



            }).ToList();
            return new ResultDto<List<GetColorDto>>
            {
                Data = ColorsList,
                IsSuccess = true,
                Message = "",

            };

        }


    }


    public class GetColorDto
    {
        public long id { get; set; }
        public string label { get; set; }


    }
}
