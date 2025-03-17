using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.ProductItems.Queries.TitleGet;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.ProductItems.Queries.TablesGet.SmdGet
{
    public interface ISmdGetService
    {
        ResultDto<List<GetSmdDto>> Execute();

    }


    public class SmdGetService : ISmdGetService
    {
        private readonly IPriceFeeDataBaseContext _context;
        public SmdGetService(IPriceFeeDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<List<GetSmdDto>> Execute()
        {
            var smds = _context.Smds;

            //int rowsCount = 0;
            var smdsList = smds.Select(p => new GetSmdDto
            {
                id = p.Id,
                label = p.SmdModel



            }).ToList();
            return new ResultDto<List<GetSmdDto>>
            {
                Data = smdsList,
                IsSuccess = true,
                Message = "",

            };

        }


    }


    public class GetSmdDto
    {
        public long id { get; set; }
        public string label { get; set; }


    }
}
