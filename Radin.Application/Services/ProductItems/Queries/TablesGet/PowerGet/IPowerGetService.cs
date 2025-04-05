//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.ProductItems.Queries.TitleGet;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.ProductItems.Queries.TablesGet.PowerGet
//{
//    public interface IPowerGetService
//    {
//        ResultDto<List<GetPowerDto>> Execute();

//    }

//    public class PowerGetService : IPowerGetService
//    {
//        private readonly IPriceFeeDataBaseContext _context;
//        public PowerGetService(IPriceFeeDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public ResultDto<List<GetPowerDto>> Execute()
//        {
//            var powers = _context.Powers;

//            //int rowsCount = 0;
//            var powersList = powers.Select(p => new GetPowerDto
//            {
//                id = p.Id,
//                label = p.PowerType.ToString()



//            }).ToList();
//            return new ResultDto<List<GetPowerDto>>
//            {
//                Data = powersList,
//                IsSuccess = true,
//                Message = "",

//            };

//        }


//    }


//    public class GetPowerDto
//    {
//        public int id { get; set; }
//        public string label { get; set; }


//    }
//}
