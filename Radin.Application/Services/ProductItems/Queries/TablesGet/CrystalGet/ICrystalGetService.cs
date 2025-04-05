//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.ProductItems.Queries.TitleGet;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.ProductItems.Queries.TablesGet.CrystalGet
//{
//    public interface ICrystalGetService
//    {
//        ResultDto<List<GetCrystalDto>> Execute();

//    }

//    public class CrystalGetService : ICrystalGetService
//    {
//        private readonly IPriceFeeDataBaseContext _context;
//        public CrystalGetService(IPriceFeeDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public ResultDto<List<GetCrystalDto>> Execute()
//        {
//            var crystals = _context.Crystals;

//            //int rowsCount = 0;
//            var crystalsList = crystals.Select(p => new GetCrystalDto
//            {
//                id = p.Id,
//                label = p.CrystalModel



//            }).ToList();
//            return new ResultDto<List<GetCrystalDto>>
//            {
//                Data = crystalsList,
//                IsSuccess = true,
//                Message = "",

//            };

//        }


//    }


//    public class GetCrystalDto
//    {
//        public int id { get; set; }
//        public string label { get; set; }


//    }
//}
