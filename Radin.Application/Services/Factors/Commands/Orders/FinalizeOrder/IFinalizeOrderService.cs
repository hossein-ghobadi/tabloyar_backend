//using CsvHelper;
//using Microsoft.EntityFrameworkCore;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Factors.Commands.UpdatePrice;
//using Radin.Application.Services.Factors.Queries.OrderGet;
//using Radin.Common;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Factors;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Factors.Commands.Orders.FinalizeOrder.FinalizeOrderService;

//namespace Radin.Application.Services.Factors.Commands.Orders.FinalizeOrder
//{
//    public interface IFinalizeOrderService
//    {
//        Task<ResultDto<List<FinalFacorResult>>> Finalize(FinalizeOrderRequest request);
//    }
//    public class FinalizeOrderService : IFinalizeOrderService
//    {
//        private readonly IDataBaseContext _context;
//        private readonly IUpdatePrice _updatePrice;

//        public FinalizeOrderService(IDataBaseContext context, IUpdatePrice updatePrice)
//        {
//            _context = context;
//            _updatePrice = updatePrice;

//        }


//        public async Task<ResultDto<List<FinalFacorResult>>> Finalize(FinalizeOrderRequest request)
//        {
//            //using (var transaction = await _context.BeginTransactionAsync())
//            //{
//            try
//            {



//                var mainFactor = await _context.MainFactors
//           .Include(mf => mf.SubFactors.Where(sf => !sf.IsRemoved)) // Include only non-removed SubFactors
//               .ThenInclude(sf => sf.ProductFactors)
//           .FirstOrDefaultAsync(mf => mf.Id == request.FactorId);

//                if (mainFactor == null)
//                {
//                    throw new Exception($"MainFactor with ID {request.FactorId} not found.");
//                }

//                bool hasTrueStatusSubFactor = false;
//                float maxSubFactorAmount = 0;
//                var ChoosedSubfactor = mainFactor.SubFactors.Where(p => p.Id == request.SubFactorId).FirstOrDefault();
//                if (ChoosedSubfactor == null)
//                {
//                    return new ResultDto<List<FinalFacorResult>> { IsSuccess = false, Message = "زیر فاکتور وجود ندارد" };
//                }


//                foreach (var subFactor in mainFactor.SubFactors)
//                {
//                    // Calculate the total amount for this SubFactor based on non-removed ProductFactors

//                    var nonRemovedProductFactors = subFactor.ProductFactors?
//                        .Where(pf => !pf.IsRemoved)
//                        .ToList() ?? new List<ProductFactor>();
//                    // If there are no non-removed ProductFactors, set the amount to zero
//                    float subFactorAmount = nonRemovedProductFactors.Any()
//                        ? nonRemovedProductFactors.Sum(pf => pf.price)
//                        : 0;

//                    // Set the calculated amount to the SubFactor
//                    subFactor.Amount = subFactorAmount;
//                    if (subFactor.Id == request.SubFactorId)
//                    {
//                        subFactor.status = true;
//                    }
//                    else
//                    {
//                        subFactor.status = false;
//                    }
//                    // Check if this SubFactor has a true status
//                    if (subFactor.status)
//                    {
//                        // Set the mainFactor's fee based on this SubFactor's amount if its status is true
//                        mainFactor.fee = subFactor.Amount;
//                        hasTrueStatusSubFactor = true;
//                    }

//                    // Track the maximum SubFactor amount for fallback if no SubFactor has a true status
//                    if (subFactor.Amount > maxSubFactorAmount)
//                    {
//                        maxSubFactorAmount = subFactor.Amount;
//                    }

//                    // Use custom method to mark the SubFactor's Amount property as modified
//                    _context.MarkAsModified(subFactor);
//                }

//                // If no SubFactors have a true status, use the maximum amount of the SubFactors
//                if (!hasTrueStatusSubFactor)
//                {
//                    mainFactor.fee = maxSubFactorAmount;
//                }

//                // If the MainFactor has no SubFactors at all, set the fee to zero
//                if (!mainFactor.SubFactors.Any())
//                {
//                    mainFactor.fee = 0;
//                }

//                // Consider packing cost and discount when setting the final amount
//                mainFactor.TotalAmount = mainFactor.fee * mainFactor.count * (1 - (mainFactor.TotalDiscount * 0.01f ?? 0))
//                                         + (mainFactor.TotalPackingCost ?? 0);

//                mainFactor.SelectedDesign = ChoosedSubfactor.Description;
//                mainFactor.state = 2;
//                // Ensure the TotalAmount is not negative
//                if (mainFactor.TotalAmount < 0)
//                {
//                    mainFactor.TotalAmount = 0;
//                }

//                // Use custom method to mark the MainFactor's fee and TotalAmount properties as modified
//                _context.MarkPropertyAsModified(mainFactor, mf => mf.fee);
//                _context.MarkPropertyAsModified(mainFactor, mf => mf.TotalAmount);
//                _context.MarkPropertyAsModified(mainFactor, mf => mf.SelectedDesign);
//                _context.MarkPropertyAsModified(mainFactor, mf => mf.state);
//                var ProductFactors = _context.ProductFactors.Where(f => f.FactorID == request.FactorId && f.SubFactorID == request.SubFactorId && f.IsRemoved == false).ToList().OrderByDescending(o => o.UpdateTime);

//                var ProductFactorsList = ProductFactors.Select(p => new FinalFacorResult
//                {
//                    id = p.Id,
//                    Name = p.Name,
//                    count = p.count,
//                    fee = p.fee,
//                    Discount = p.Discount,

//                    InsertTime = p.InsertTime,
//                    price = p.count * p.fee * (1 - p.Discount * 0.01f),
//                }).ToList();
//                // Save all changes to the database
//                await _context.SaveChangesAsync();
//                return new ResultDto<List<FinalFacorResult>>
//                {
//                    Data = ProductFactorsList,
//                    IsSuccess = true,
//                    Message = "فاکتور مورد نظر آماده پرداخت است"
//                };
//            }







//            catch
//            {
//                //await transaction.RollbackAsync();
//                return new ResultDto<List<FinalFacorResult>>
//                {
//                    IsSuccess = false,
//                    Message = "An error occurred while processing the request."
//                };
//            }
//        }

//    }
//    public class FinalizeOrderRequest
//    {
//        public long FactorId { get; set; }
//        public long SubFactorId { get; set; }

//    }
//    public class FinalFacorResult
//    {
//        public long id { get; set; }
//        public string Name { get; set; }
//        public int count { get; set; }
//        public float fee { get; set; }
//        public float Discount { get; set; }
//        public float price { get; set; }
//        public DateTime InsertTime { get; set; }
//    }
//}