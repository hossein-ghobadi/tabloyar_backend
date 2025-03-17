using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Commands.UpdatePrice;
using Radin.Common;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Factors.Commands.Orders.OrdersRemove.ProductFactorRemove;

namespace Radin.Application.Services.Factors.Commands.Orders.OrdersRemove
{
    public interface IProductFactorRemove
    {
        Task<ResultDto> ExecuteAsync(ProductFactorRemoveRequest request);
    }

    public class ProductFactorRemove : IProductFactorRemove
    {
        private readonly IDataBaseContext _context;
        private readonly IUpdatePrice _updatePrice;

        public ProductFactorRemove(IDataBaseContext context,

            IUpdatePrice updatePrice)
        {
            _context = context;
            _updatePrice = updatePrice;

        }


        public async Task<ResultDto> ExecuteAsync(ProductFactorRemoveRequest request)
        {
            using (var transaction = await _context.BeginTransactionAsync())
            {
                try
                {
                    var ProductFactor = _context.ProductFactors.FirstOrDefault(p => p.Id == request.ProductFactorId);
                    Console.WriteLine($@"ID={request.ProductFactorId}");
                    Console.WriteLine($@"Factor={request.ProductFactorId}");

                    if (ProductFactor == null)
                    {
                        return new ResultDto()
                        {
                            IsSuccess = false,
                            Message = "فاکتور مورد نظر یافت نشد"
                        };
                    }
                    ProductFactor.RemoveTime = DateTime.Now;
                    ProductFactor.IsRemoved = true;

                    await _context.SaveChangesAsync();
                    await _updatePrice.UpdateFactorPricesAsync(ProductFactor.FactorID);
                    await transaction.CommitAsync();


                    return new ResultDto()
                    {
                        IsSuccess = true,
                        Message = "فاکتور مورد نظر با موفققیت حذف شد"
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "An error occurred while processing the request."
                    };
                }
            }

        }

       
        public class ProductFactorRemoveRequest
        {
            public long ProductFactorId { get; set; }
        }
    }
}
