//using Microsoft.EntityFrameworkCore;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Commands.ContentRemove;
//using Radin.Common;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Factors.Commands.Orders.OrdersRemove.FactorRemoveService;

//namespace Radin.Application.Services.Factors.Commands.Orders.OrdersRemove
//{
//    public interface IFactorRemoveService
//    {
//        ResultDto Execute(FactorRemoveRequest request);
//    }

//    public class FactorRemoveService : IFactorRemoveService
//    {
//        private readonly IDataBaseContext _context;

//        public FactorRemoveService(IDataBaseContext context)
//        {
//            _context = context;
//        }


//        public ResultDto Execute(FactorRemoveRequest request)
//        {

//            var factor = _context.MainFactors.FirstOrDefault(f => f.Id == request.FactorId);


//            if (factor == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "فاکتور مورد نظر یافت نشد"
//                };
//            }

//            factor.RemoveTime = DateTime.Now;
//            factor.IsRemoved = true;

//            var SubFactors = _context.SubFactors.Where(s => s.FactorID == request.FactorId );
//            if (SubFactors != null)
//            {
//                foreach (var subFactor in SubFactors)
//                {
//                    subFactor.RemoveTime = DateTime.Now;
//                    subFactor.IsRemoved = true;
//                }
//            }

//            var Products = _context.ProductFactors.Where(p => p.FactorID == request.FactorId);
//            if (Products != null)
//            {
//                foreach (var product in Products)
//                {
//                    product.RemoveTime = DateTime.Now;
//                    product.IsRemoved = true;
//                }
//            }
//            _context.SaveChanges();
//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message = "فاکتور مورد نظر با موفققیت حذف شد"
//            };
//        }
        
      
//        public class FactorRemoveRequest
//        {
//            public long FactorId { get; set; }
//        }

//    }
//}
