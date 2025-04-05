//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.ProductItems.Queries.TitleGet;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.ProductItems.Queries.TablesGet.EdgeSizeGet
//{
//    public interface IEdgeSizeGetService
//    {
//        ResultDto<List<GetEdgeSizeDto>> Execute(string request);

//    }


//    public class EdgeSizeGetService : IEdgeSizeGetService
//    {
//        private readonly IPriceFeeDataBaseContext _context;
//        public EdgeSizeGetService(IPriceFeeDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public ResultDto<List<GetEdgeSizeDto>> Execute(string request)
//        {
//            var edgeSizes = _context.MaterialEdgeSizes;

//            //int rowsCount = 0;
//            var edgeSizesList = edgeSizes.Where(p => p.Title.Contains(request)).Select(p => new GetEdgeSizeDto
//            {
//                id = p.Id,
//                label = p.EdgeSize.ToString(),



//            }).ToList();
//            return new ResultDto<List<GetEdgeSizeDto>>
//            {
//                Data = edgeSizesList,
//                IsSuccess = true,
//                Message = "",

//            };

//        }


//    }


//    public class GetEdgeSizeDto
//    {
//        public long id { get; set; }
//        public string label { get; set; }


//    }
//}
