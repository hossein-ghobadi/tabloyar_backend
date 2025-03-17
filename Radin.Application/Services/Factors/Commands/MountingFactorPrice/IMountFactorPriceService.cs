using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Drawing.Style.Fill;
using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.PolicyRequirementsService;

namespace Radin.Application.Services.Factors.Commands.MountingFactorPrice
{
    public interface IMountFactorPriceService
    {
        ResultDto Execute(long FactorId);
    }

    public class MountFactorPriceService : IMountFactorPriceService
    {

        private readonly IDataBaseContext _context;

        public MountFactorPriceService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long FactorId)
        {
            var Factor =  _context.MainFactors
                                               .Include(m => m.SubFactors)
                                               .FirstOrDefaultAsync(p => p.Id == FactorId && !p.IsRemoved);
            if (Factor!=null)
            {
                var subfactors = Factor.Result.SubFactors.AsQueryable();
                if (!subfactors.Any())
                {
                    Factor.Result.fee = 0;
                    Factor.Result.count = 1;
                    Factor.Result.TotalAmount = 0;
                    Factor.Result.TotalDiscount = 0;
                    _context.SaveChanges();
                    return new ResultDto
                    {
                        IsSuccess = true,
                        Message = "فاکتور بدون زیر فاکتور بود"
                    };
                }
                else
                {
                    var TrueSubfactor = subfactors.Where(p => p.status && !p.IsRemoved);
                    if (TrueSubfactor.Any()){
                        Factor.Result.fee = TrueSubfactor.FirstOrDefault().Amount;
                        Factor.Result.TotalAmount = Factor.Result.fee * Factor.Result.count * (1 - Factor.Result.TotalDiscount);
                        _context.SaveChanges();
                        return new ResultDto
                        {
                            IsSuccess = true,
                            Message = "قیمت فاکتور آپدیت شد"
                        };
                    }
                    var maxSubFactor = Factor.Result.SubFactors.Where(p=>!p.IsRemoved )
                    .OrderByDescending(p => p.Amount)
                    .FirstOrDefault();
                    if (maxSubFactor == null) {

                        return new ResultDto
                        {
                            IsSuccess = false,
                            Message = "تغییراتی انجام نشد"
                        };

                    }
                    Factor.Result.fee = maxSubFactor.Amount;
                    
                    Factor.Result.TotalAmount = Factor.Result.fee* Factor.Result.count*(1- Factor.Result.TotalDiscount);
                    _context.SaveChanges();
                    return new ResultDto
                    {
                        IsSuccess = true,
                        Message = "تغییرات قیمت فاکتور انجام شد"
                    };
                }
                

            }
            return new ResultDto
            {
                IsSuccess = false,
                Message = "فاکتور موجود نیست"
            };

        }
    }
}
