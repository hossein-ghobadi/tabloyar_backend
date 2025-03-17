using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Commands.MountingFactorPrice;
using Radin.Application.Services.Factors.Commands.UpdatePrice;
using Radin.Common;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Factors.Commands.Orders.OrdersRemove.SubFactorRemoveService;

namespace Radin.Application.Services.Factors.Commands.Orders.OrdersRemove
{
    public interface ISubFactorRemoveService
    {
        Task<ResultDto> Execute(SubFactorRemoveRequest request);
    }

    public class SubFactorRemoveService : ISubFactorRemoveService
    {
        private readonly IDataBaseContext _context;
        private readonly IUpdatePrice _updatePrice;
        public SubFactorRemoveService(IDataBaseContext context,

            IUpdatePrice updatePrice
            )
        {
            _context = context;
            _updatePrice = updatePrice;
        }


        public async Task<ResultDto> Execute(SubFactorRemoveRequest request)
        {
            using (var transaction = await _context.BeginTransactionAsync())
            {
                try
                {
                    var SubFactor = _context.SubFactors.FirstOrDefault(f => f.Id == request.SubFactorId);


                    if (SubFactor == null)
                    {
                        return new ResultDto
                        {
                            IsSuccess = false,
                            Message = "فاکتور مورد نظر یافت نشد"
                        };
                    }



                    var FactorId = SubFactor.FactorID;
                    var Factor = _context.MainFactors.FirstOrDefault(f => f.Id == FactorId && !f.IsRemoved);
                    if (Factor == null)
                    {
                        return new ResultDto
                        {
                            IsSuccess = false,
                            Message = "فاکتور مربوطه ایراد دارد، تمام بخش های آن را چک کنید"
                        };
                    }
                    if (SubFactor.status == true)
                    {
                        SubFactor.status = false;
                        Factor.status = false;

                    }






                    SubFactor.RemoveTime = DateTime.Now;
                    SubFactor.IsRemoved = true;

                    var Products = _context.ProductFactors.Where(p => p.SubFactorID == request.SubFactorId);
                    if (Products != null)
                    {
                        foreach (var product in Products)
                        {
                            product.RemoveTime = DateTime.Now;
                            product.IsRemoved = true;
                        }
                    }
                    await _context.SaveChangesAsync();
                    await _updatePrice.UpdateFactorPricesAsync(FactorId);

                    
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




        public class SubFactorRemoveRequest
        {
            public long SubFactorId { get; set; }
        }
    }
}
