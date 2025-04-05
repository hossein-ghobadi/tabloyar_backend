//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.ProductItems.Queries.TitleGet;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.ProductItems.Queries.TablesGet.MarginGet
//{
//    public interface IMarginGetService
//    {
//        ResultDto<List<GetMarginDto>> Execute();

//    }


//    public class MarginGetService : IMarginGetService
//    {
//        private readonly IPriceFeeDataBaseContext _context;
//        public MarginGetService(IPriceFeeDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public ResultDto<List<GetMarginDto>> Execute()
//        {
//            var margins = _context.Margins;

//            //int rowsCount = 0;
//            var marginsList = margins.Select(p => new GetMarginDto
//            {
//                id = p.Id,
//                label = p.MarginNumber



//            }).ToList();
//            return new ResultDto<List<GetMarginDto>>
//            {
//                Data = marginsList,
//                IsSuccess = true,
//                Message = "",

//            };

//        }


//    }


//    public class GetMarginDto
//    {
//        public int id { get; set; }
//        public float label { get; set; }


//    }
//}
