//using Radin.Application.Interfaces.Contexts;
//using Radin.Common.Dto;
//using Radin.Common.StaticClass;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Radin.Common.StaticClass;
//using Microsoft.AspNetCore.Identity;
//using Radin.Domain.Entities.Users;
//using Radin.Domain.Entities.Factors;
//using static Radin.Application.Services.Factors.Commands.FactorContractSet.ProductsData;
//using Radin.Application.Services.Factors.Queries.FactorContractGet;
//using System.Reflection.Metadata.Ecma335;

//namespace Radin.Application.Services.Factors.Commands.FactorContractSet
//{
//    public interface IFactorContractSetService
//    {
//        ResultDto<object> ContractSet(ContractSetRequestDto request);

//    }
//    public class FactorContractSetService : IFactorContractSetService
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly IFactorContractGet _factorContractGet;
//        private readonly IDataBaseContext _context;
//        public FactorContractSetService(IDataBaseContext context, UserManager<User> userManager, IFactorContractGet factorContractGet)
//        {
//            _context = context;
//            _userManager = userManager;
//            _factorContractGet = factorContractGet;

//        }
//        public ResultDto<object> ContractSet(ContractSetRequestDto request)
//        {


//            var factor = _context.MainFactors.FirstOrDefault(p => p.Id == request.factorId);
//            if (factor == null) { return new ResultDto<object> { IsSuccess = false, Message = "فاکتور وجود ندارد" }; }
//            var subfactor = _context.SubFactors.FirstOrDefault(p => p.FactorID == request.factorId && p.status == true);
//            if (subfactor == null) { return new ResultDto<object> { IsSuccess = false, Message = "زیر فاکتور وجود ندارد" }; }
//            var Products = _context.ProductFactors.Where(p => p.SubFactorID == subfactor.Id && !p.IsRemoved).ToList();
//            var factorDetails = Products.Select(p => new ProductsData
//            {
//                id = p.Id,
//                name = p.Name,
//                price = p.price.ToString(),
//                fee = p.fee.ToString(),
//                discount = p.Discount.ToString(),
//                number=p.count.ToString(),
//            }).ToList();

//            var productSelectios=new List<ProductSelectionResult>();
//            foreach(var product in Products)
//            {
//                if (!product.IsUndefinedProduct && !product.IsAccessory && !product.IsService)
//                {
//                    var tempResut = _factorContractGet.GetFactorProductSelection(product.Id);
//                    if (tempResut.IsSuccess) { productSelectios.Add(tempResut.Data); }
//                    else { return new ResultDto<object> { IsSuccess = false, Message = tempResut.Message }; }
//                }
//            }
//            var ContractDate = DateTime.Now;
//            TimeZoneInfo tehranTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");

//            // Ensure the DateValue is treated as UTC
//            ContractDate = DateTime.SpecifyKind(ContractDate, DateTimeKind.Utc);

//            DateTime tehranTime = TimeZoneInfo.ConvertTimeFromUtc(ContractDate, tehranTimeZone);

//            var Result = new ContractSetResultDto();
            

//            var Seller = _userManager.FindByIdAsync(factor.MainsellerID).Result;
//            var Customer = _context.CustomerInfo.FirstOrDefault(p => p.CustomerID == Convert.ToInt64(request.customerId));
//            if (Seller == null) { return new ResultDto<object> { IsSuccess = false, Message = "اطلاعات فروشنده وجود ندارد" }; }
//            if (Customer == null) { return new ResultDto<object> { IsSuccess = false, Message = "اطلاعات مشتری وجود ندارد" }; }
//            var factorContract = _context.FactorContracts.FirstOrDefault(p => p.factorId == request.factorId);


//            Result.factorId = request.factorId; 
//            Result.craneCost = request.craneCost;
//            Result.scaffoldCost = request.scaffoldCost;
//            Result.transportationCost = request.transportationCost;
//            Result.customerAdress = request.customerAdress;
//            Result.customerNationalNumber = request.customerNationalNumber;
//            Result.customerName = request.customerName;
//            Result.warrantyMonthNumber = request.warrantyMonthNumber+1;
//            Result.attachedNumber = request.attachedNumber;
//            Result.customerPhone = request.customerPhone;
//            Result.contractDate = ContractDate;
//            Result.deliveryDate= request.deliveryDate;
//            Result.sellerName = Seller.FullName;
//            Result.sellerPhone = Seller.PhoneNumber.ToString();
//            if (factorContract != null)
//            {
//                factorContract.factorId = request.factorId; 
//                factorContract.craneCost = request.craneCost;
//                factorContract.scaffoldCost = request.scaffoldCost;
//                factorContract.transportationCost = request.transportationCost;
//                factorContract.sellerName = Seller.FullName;
//                factorContract.sellerId = factor.MainsellerID;
//                factorContract.customerAdress = request.customerAdress;
//                factorContract.customerNationalNumber = request.customerNationalNumber;
//                factorContract.customerName = request.customerName;
//                factorContract.warrantyMonthNumber = request.warrantyMonthNumber+1;
//                factorContract.attachedNumber = request.attachedNumber;
//                factorContract.customerPhone = request.customerPhone;
//                factorContract.customerId = request.customerId;
//                factorContract.deliveryDate = request.deliveryDate;
//                _context.SaveChanges();


//            }
//            else
//            {
//                FactorContract Contract = new FactorContract
//                {
//                    factorId = request.factorId,

//                craneCost = request.craneCost,
//                    scaffoldCost = request.scaffoldCost,
//                    transportationCost = request.transportationCost,
//                    sellerName = Seller.FullName,
//                    sellerId = factor.MainsellerID,
//                    customerAdress = request.customerAdress,
//                    customerNationalNumber = request.customerNationalNumber,
//                    customerName = request.customerName,
//                    warrantyMonthNumber = request.warrantyMonthNumber,
//                    attachedNumber = request.attachedNumber,
//                    customerPhone = request.customerPhone,
//                    customerId = request.customerId,
//                    deliveryDate= request.deliveryDate

//                };
//                _context.FactorContracts.Add(Contract);
//                _context.SaveChanges();

//            }
//            Result.factorDetails = factorDetails;
//            Result.SelectingDetails = productSelectios;
//            return new ResultDto<object> {Data= Result, IsSuccess = true, Message = "دریافت  موفقیت آمیز قرارداد" };
//        }

        
//    }



//    public class ProductsData
//    {
//        public long id { get; set; }
//        public string name { get; set; }
//        public string fee { get; set; }
//        public string number { get; set; }
//        public string discount { get; set; }
//        public string price { get; set; }
//    }
//    public class ContractSetRequestDto
//    {
//        public long factorId { get; set; }
//        public string customerPhone { get; set; }
//        public string customerId { get; set; }
//        public string customerNationalNumber { get; set; }
//        public string customerName { get; set; }
//        public string customerAdress { get; set; }
//        public bool scaffoldCost { get; set; }/// TRUE>>>>پیمانکار
//        public bool craneCost { get; set; }/// TRUE>>>>پیمانکار
//        public bool transportationCost { get; set; }/// TRUE>>>>پیمانکار
//        public int warrantyMonthNumber { get; set; }
//        public int attachedNumber { get; set; }
//        public int deliveryDate { get; set; }



//    }


//    public class ContractSetResultDto
//    {
//        public long factorId { get; set; }
//        public string customerPhone { get; set; }//
//        public string customerNationalNumber { get; set; }//
//        public string customerName { get; set; }//
//        public string customerAdress { get; set; }//
//        public int deliveryDate { get; set; }//
//        public string sellerName { get; set; }
//        public string sellerPhone { get; set; }
//        public bool scaffoldCost { get; set; }/// TRUE>>>>پیمانکار
//        public bool craneCost { get; set; }/// TRUE>>>>پیمانکار
//        public bool transportationCost { get; set; }/// TRUE>>>>پیمانکار
//        public int warrantyMonthNumber { get; set; }//
//        public DateTime contractDate { get; set; }//
//        public int attachedNumber { get; set; }
//        public List<ProductsData> factorDetails { get; set; }
//        public List<ProductSelectionResult> SelectingDetails { get; set; }

//    }


//}
    