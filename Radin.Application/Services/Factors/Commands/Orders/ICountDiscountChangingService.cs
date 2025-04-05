//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Factors.Commands.UpdatePrice;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Factors.Commands.Orders
//{
//    public interface ICountDiscountChangingService
//    {
//        Task<ResultDto> ExecuteAsync(RequestCountDiscountChanging request);
//    }

//    public class CountDiscountChangingService : ICountDiscountChangingService
//    {
//        private readonly IDataBaseContext _context;
//        private readonly IUpdatePrice _updatePrice;

//        public CountDiscountChangingService(IDataBaseContext context, IUpdatePrice updatePrice)
//        {
//            _context = context;
//            _updatePrice = updatePrice;

//        }


//        public async Task<ResultDto> ExecuteAsync(RequestCountDiscountChanging request)
//        {
//            var product = _context.ProductFactors.Where(p => p.Id == request.ProductID).FirstOrDefault();



//            if (product == null)
//            {
//                return new ResultDto

//                {

//                    IsSuccess = false,
//                    Message = "چنین فاکتوری وجود ندارد",
//                };
//            }

//            using (var transaction = await _context.BeginTransactionAsync())
//            {
//                try
//                {
//                    // Update the product details
//                    product.count = request.count;
//                    product.Discount = request.discount;
//                    product.price= request.count*product.fee*(1-request.discount*0.01f);
//                    // Save changes to the database
//                    _context.SaveChanges();
//                    await _updatePrice.UpdateFactorPricesAsync(product.FactorID);
//                    // Commit the transaction
//                    await transaction.CommitAsync();

//                    return new ResultDto
//                    {
//                        IsSuccess = true,
//                        Message = "تغییرات با موفقیت اعمال شد",
//                    };
//                }
//                catch (Exception ex)
//                {
//                    // Rollback the transaction in case of an error
//                    await transaction.RollbackAsync();

//                    return new ResultDto
//                    {
//                        IsSuccess = false,
//                        Message = "خطایی رخ داد: " + ex.Message,
//                    };
//                }
//            }

//        }
        
//    }



//    public class RequestCountDiscountChanging
//    {
//        public long ProductID { get; set; }
//        public int count { get; set; }
//        public float discount { get; set; }
//    }
//}
