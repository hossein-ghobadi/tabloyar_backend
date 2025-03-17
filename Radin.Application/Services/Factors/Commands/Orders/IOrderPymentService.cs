//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Factors.Commands.Orders.OrdersRemove;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Factors.Commands.Orders.OrderPymentService;

//namespace Radin.Application.Services.Factors.Commands.Orders
//{
//    public interface IOrderPymentService
//    {
//        ResultDto Execute(FactorsIdRequest request);
//    }

//    public class OrderPymentService : IOrderPymentService
//    {
//        private readonly IDataBaseContext _context;

//        public OrderPymentService(IDataBaseContext context)
//        {
//            _context = context;
//        }


//        public ResultDto Execute(FactorsIdRequest request)
//        {

//            var factor = _context.MainFactors.FirstOrDefault(f => f.Id == request.FactorId && f.IsRemoved == false);

//            if (factor == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "فاکتور مورد نظر یافت نشد"
//                };
//            }
//            if (factor.state != 2)
//            {
//                factor.state = 2;
//            }
//            var SubFactors = _context.SubFactors.FirstOrDefault(s => s.FactorID == request.SUbFactorId && s.IsRemoved == false);

//            if (SubFactors == null)
//            {
//                return new ResultDto
//                {
//                    IsSuccess = false,
//                    Message = "محصول مورد نظر یافت نشد"
//                };
//            }

//            //if (Products != null)
//            //{
//            //    foreach (var product in Products)
//            //    {
//            //        product.RemoveTime = DateTime.Now;
//            //        product.IsRemoved = true;
//            //    }
//            //}
//            _context.SaveChanges();
//            return new ResultDto()
//            {
//                IsSuccess = true,
//                Message = "فاکتور مورد نظر با موفققیت حذف شد"
//            };
//        }

//        public class FactorsIdRequest
//        {
//            public long FactorId { get; set; }
//            public long SUbFactorId { get; set; }
//        }

//    }
//}
