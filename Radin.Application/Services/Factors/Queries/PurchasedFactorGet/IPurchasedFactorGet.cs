//using CsvHelper;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Common;
//using Radin.Common.Dto;
//using Radin.Common.HesabfaItems;
//using Radin.Domain.Entities.Factors;
//using Radin.Domain.Entities.Users;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Factors.Queries.PurchasedFactorGet.PurchasedFactorGet;

//namespace Radin.Application.Services.Factors.Queries.PurchasedFactorGet
//{
//    public interface IPurchasedFactorGet
//    {
//        ResultDto<List<AbstractPurchaseResult>> AbstractOfPurchased(long? branchCode);
//        ResultDto<List<ProductInformation>> PurchasedProducts(long factorId);

//    }
//    public class PurchasedFactorGet : IPurchasedFactorGet
//    {
//        private readonly IDataBaseContext _context;
//        private readonly UserManager<User> _userManager;

//        public PurchasedFactorGet(IDataBaseContext context, UserManager<User> userManager)
//        {
//            _context = context;
//            _userManager = userManager;

//        }
//        public ResultDto<List<AbstractPurchaseResult>> AbstractOfPurchased(long? branchCode)
//        {
//            try 
            
//            { 
//                if (branchCode != null) { }
           

//                var branches = _context.BranchINFOs.Select(p=> new IdLabelDto { id=Convert.ToInt32(p.BranchCode),label=p.BranchName}).ToList();
//                Dictionary<int, string> branchDictionary = branches.ToDictionary(b => b.id, b => b.label);

//                var previousTwoMonth = DateTime.Now.AddMonths(-1);
//                var InitialFactors = _context.MainFactors.Where(p => p.InitialConnectionTime > previousTwoMonth && p.position  && p.CustomerID != null && !p.IsRemoved);
//                if (branchCode != null) { InitialFactors = InitialFactors.Where(p => p.BranchCode == branchCode); }
//                var PurchasedFactors = InitialFactors
//                        .Where(f => _context.ProductFactors.Any(p => p.FactorID == f.Id && !p.IsRemoved)).Join(_context.CustomerInfo,
//                        Fact => Fact.CustomerID,
//                        Cust => Cust.CustomerID,
//                        (f, c) => new
//                        {
//                            purchaseTime=f.InitialConnectionTime,
//                            FactorId = f.Id,
//                            WorkName=f.WorkName,
//                            CustomerId = f.CustomerID,
//                            CustomerPhone = c.phone,
//                            CustomerName = $"{c.Name} {c.LastName}",
//                            CustomerCity = c.city,
//                            SellerId = f.MainsellerID,
//                            FactorPrice = f.TotalAmount,
//                            PaymentCondition = SwithPaymentType(f.PaymentType),
//                            BranchCode=f.BranchCode,
//                        }

//                        ).ToList();

                
//                var FullData = PurchasedFactors.OrderByDescending(p=>p.purchaseTime).Join(_userManager.Users,
//                    Factor => Factor.SellerId,
//                    Seller => Seller.Id,
//                    (f, s) => 
//                    {
//                        string branchName;
//                        branchDictionary.TryGetValue(Convert.ToInt32(f.BranchCode), out branchName);
//                        return new AbstractPurchaseResult
//                        {
//                            factorId = f.FactorId,
//                            branchName = branchName,
//                            workName = f.WorkName,
//                            paymentCondition = f.PaymentCondition,
//                            factorPrice = f.FactorPrice.ToString(),
//                            paymentsReceipt=new List<payReport>(),
//                            factorInformation = new BranchData
//                            {
//                                sellerName = s.FullName,
//                                sellerPhone = s.PhoneNumber,
//                                customerName = f.CustomerName,
//                                customerPhone = f.CustomerPhone,
//                                customerCity = _context.Cities.First(p => p.Id == f.CustomerCity).city

//                            }
                            
//                        };

//                    }


//                    ).ToList();
//                var factorIds = FullData.Select(f => f.factorId).ToList();

//                // Fetch payment reports for all factors in one query
//                var paymentReports = _context.PaymentReports
//                    .Where(p => factorIds.Contains(p.FactorId))
//                    .ToDictionary(p => p.FactorId);

//                // Fetch check payments in one go
//                var checkPayments = _context.CheckPayments
//                    .Where(p => paymentReports.Values.Select(pr => pr.Id).Contains(p.PaymentId))
//                    .ToList();

//                // Attach payment data to FullData
//                foreach (var factor in FullData)
//                {
//                    if (paymentReports.TryGetValue(factor.factorId, out var cashReport))
//                    {
//                        if (cashReport.CashInitialPayment != 0) {
//                            factor.paymentsReceipt.Add(new payReport
//                            {
//                                id = 1,
//                                price = cashReport.CashInitialPayment,
//                                receipt = cashReport.PaymentReceiptImage
//                            });
//                        }

//                        var relatedChecks = checkPayments.Where(cp => cp.PaymentId == cashReport.Id).ToList();
//                        int payId = 2;
//                        foreach (var checkReport in relatedChecks)
//                        {
//                            factor.paymentsReceipt.Add(new payReport
//                            {
//                                id = payId++,
//                                price = checkReport.CheckPrice,
//                                receipt = cashReport.PaymentReceiptImage,
//                                payType = "چک"
//                            });
//                        }
//                    }
//                    else
//                    {
//                        return new ResultDto<List<AbstractPurchaseResult>>
//                        {
//                            IsSuccess = false,
//                            Message = "اطلاعات پرداخت نقدی وجود ندارد"
//                        };
//                    }
//                }
//                    return new ResultDto<List<AbstractPurchaseResult>>
//                {
//                    Data = FullData,
//                    IsSuccess = true,
//                    Message = "دریافت موفق"
//                };


//            }
//            catch {
//                return new ResultDto<List<AbstractPurchaseResult>>
//                {
//                    IsSuccess = false,
//                    Message = "خطا دریافت"
//                };

//            }
//        }



//        //

//        public ResultDto<List<ProductInformation>> PurchasedProducts(long factorId)
//        {
//            try

//            {
//                var Factor = _context.MainFactors.FirstOrDefault(p => !p.IsRemoved && p.Id == factorId);
//                if (Factor == null)
//                {
//                    return new ResultDto<List<ProductInformation>>
//                    {
//                        IsSuccess = false,
//                        Message = "چنین فاکتوری وجود ندارد"
//                    };
//                }
//                var subfactors = _context.SubFactors.Where(p =>p.FactorID==factorId&& p.status == true&& !p.IsRemoved).ToList();
//                if (subfactors.Count != 1)
//                {
//                    return new ResultDto<List<ProductInformation>>
//                    {
//                        IsSuccess = false,
//                        Message = "زیر فاکتور مورد نظر یافت نشد"
//                    };
//                }
//                var subfactorId = subfactors.First().Id;
//                var FactorProducts=_context.ProductFactors.Where(p=>!p.IsRemoved&&p.FactorID==factorId&& p.SubFactorID==subfactorId&&!p.IsUndefinedProduct&&!p.IsAccessory&&!p.IsService).ToList();
//                if (FactorProducts == null)
//                {
//                    return new ResultDto<List<ProductInformation>>
//                    {
//                        IsSuccess = false,
//                        Message = "محصولی وجود ندارد"
//                    };
//                }
                
//                var FactorList = FactorProducts.Select(p => new ProductInformation
//                {
//                    productId=p.Id.ToString(), productName=p.Name,workName=Factor.WorkName,svgName=$"{Factor.WorkName}_{p.Name}_{p.Id}",svgAdress=p.ProductDetails


//                }).ToList();
//                return new ResultDto<List<ProductInformation>>
//                {
//                    Data= FactorList,
//                    IsSuccess = true,
//                    Message = "دریافت موفق"
//                };


//            }
//            catch
//            {
//                return new ResultDto<List<ProductInformation>>
//                {
//                    IsSuccess = false,
//                    Message = "خطا دریافت"
//                };

//            }
//        }







//        private static string SwithPaymentType(int number)
//        {
//            switch (number)
//            {
//                case 0:
//                    return PaymentTypes.CashPayment;
                    
//                case 1:
//                    return PaymentTypes.SemiCashPayment;
//                case 2:
//                    return PaymentTypes.CheckPayment;
//                default:
//                    return PaymentTypes.CashPayment;
//            }
//        }

//        public class AbstractPurchaseResult
//        {
//            public long factorId {  get; set; } 
//            public string branchName {  get; set; }
//            public string workName { get; set; }
//            public string paymentCondition { get; set; }
//            public string factorPrice { get; set; }
//            public List<payReport> paymentsReceipt { get; set; }
//            public BranchData factorInformation { get; set; }
//        }
//        public class payReport
//        {
//            public long id { get; set; }
//            public string receipt { get; set; }
//            public float price { get; set; }
//            public string payType { get; set; } = "نقدی";
//        }
//        public class BranchData
//        {
//            public string sellerName { get; set; }//
//            public string sellerPhone { get; set; }//
//            public string customerName { get; set; }
//            public string customerPhone { get; set; }
//            public string customerCity { get; set; } //


//        }



//        public class ProductInformation
//        {
//            public string productId { get; set; }//
//            public string productName { get; set; }//
//            public string workName { get; set; }
//            public string? svgAdress { get; set; }
//            public string svgName { get; set; }


//        }

//    }





//}
