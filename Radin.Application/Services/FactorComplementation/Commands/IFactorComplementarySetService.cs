//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.FactorComplementation.Queries;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Factors;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.FactorComplementation.Commands.FactorComplementarySetService;

//namespace Radin.Application.Services.FactorComplementation.Commands
//{
//    public interface IFactorComplementarySetService
//    {
//        Task<ResultDto> SetComplementary(SetFactorComplementaryRequest request);
//        Task<ResultDto> RemoveComplementary(RequestId request);


//    }
//    public class FactorComplementarySetService : IFactorComplementarySetService
//    {
//        private readonly IDataBaseContext _context;

//        //private static readonly HttpClient client = new HttpClient();

//        public FactorComplementarySetService(IDataBaseContext context, IPriceFeeDataBaseContext context2)
//        {
//            _context = context;
//        }

//        public async Task<ResultDto> SetComplementary(SetFactorComplementaryRequest request)
//        {


//            var product = _context.ProductFactors.FirstOrDefault(p => p.Id == request.productId);
//            if (product == null) { return new ResultDto { IsSuccess = false, Message = "چنین محصولی وجود ندارد" }; }
//            var productId = product.Id;
//            var factorId = product.FactorID;
//            var item = new FactorProductComplementary
//            {
//                FactorId = factorId,
//                ProductId = productId,
//                ComplementaryId = request.complementaryType,
//                FirstArg = request.firstArg,
//                SecondArg = request.secondArg,
//                Description = request.description,
//            };
//            _context.FactorProductComplementaries.Add(item);
//            _context.SaveChanges();

//            return new ResultDto { IsSuccess = true, Message = "ثبت موفق" };
//        }




//        public async Task<ResultDto> RemoveComplementary(RequestId request)
//        {


//            var item = _context.FactorProductComplementaries.FirstOrDefault(p => p.Id == request.Id);
//            if (item == null) { return new ResultDto { IsSuccess = false, Message = "چنین ایتمی وجود ندارد" }; }
//            _context.FactorProductComplementaries.Remove(item);
//            _context.SaveChanges();

//            return new ResultDto { IsSuccess = true, Message = "حذف موفق" };
//        }

//    }
//    public class SetFactorComplementaryRequest
//    {
//        public long productId { get; set; }
//        public int complementaryType { get; set; }
//        public string firstArg { get; set; }
//        public string? secondArg { get; set; } = null;
//        public string description { get; set; } = "";
//    }
//}
