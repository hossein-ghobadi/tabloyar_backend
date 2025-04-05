//using Microsoft.AspNetCore.Identity;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Common;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Factors.Queries.OrderGet
//{
//    public interface IGetProductFactors
//    {
//        ResultDto<ProductFactorGetResult> Execute(ProductFactorGetRequest request);
//        ResultDto<ProductFactorComplementationResult> GetProductForComplementation(long factorId);

//    }

//    public class GetProductFactors : IGetProductFactors
//    {

//        private readonly IDataBaseContext _context;
//        public GetProductFactors (IDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public ResultDto<ProductFactorGetResult> Execute(ProductFactorGetRequest request)
//        {
//            var factor = _context.MainFactors.FirstOrDefault(i => i.Id == request.FactorId && i.IsRemoved == false);
//            if (factor == null)
//            {

//                var ProductFactorsList1 = new List<ProductFactorGetDto>
//                {
//                    new ProductFactorGetDto
//                    {
//                    Name = "",
//                    count = 0,
//                    fee = 0,
//                    Discount = 0,
                    
//                    InsertTime = DateTime.Now
//                    }
//                };
//                return new ResultDto<ProductFactorGetResult>()
//                {
//                    Data = new ProductFactorGetResult
//                    {
//                        ProductFactorsInfo = ProductFactorsList1
//                    },
//                    IsSuccess = false,
//                    Message = " فاکتوری وجود ندارد "

//                };

//            }
            
//            var subfactors = _context.ProductFactors.AsQueryable();
//            var ProductFactors = _context.ProductFactors.Where(f => f.FactorID == request.FactorId && f.SubFactorID == request.SubFactorID && f.IsRemoved == false&& f.IsAccessory==request.IsAccessory&& !f.IsUndefinedProduct && !f.IsService).ToList().OrderByDescending(o => o.UpdateTime);

//            var ProductFactorsList = ProductFactors.Select(p => new ProductFactorGetDto
//            {
//                id= p.Id,
//                Name = p.Name,
//                count = p.count,
//                fee = request.QualityFactor switch
//                {
//                    ConstantMaterialName.QualityFactor_A2plus => p.priceA2plus,
//                    ConstantMaterialName.QualityFactor_Aplus => p.priceAplus,
//                    ConstantMaterialName.QualityFactor_A => p.priceA,
//                    ConstantMaterialName.QualityFactor_B => p.priceB,
//                    _ => 0 // Default case if QF doesn't match
//                } * (1 - p.Discount * 0.01f),
//                purchaseFee= request.QualityFactor switch
//                {
//                    ConstantMaterialName.QualityFactor_A2plus => p.priceA2plus,
//                    ConstantMaterialName.QualityFactor_Aplus => p.priceAplus,
//                    ConstantMaterialName.QualityFactor_A => p.priceA,
//                    ConstantMaterialName.QualityFactor_B => p.priceB,
//                    _ => 0 // Default case if QF doesn't match
//                }/2,
//                Discount = p.Discount,
                
//                InsertTime = p.InsertTime,
//                price=p.count * (request.QualityFactor switch
//                {
//                    ConstantMaterialName.QualityFactor_A2plus => p.priceA2plus,
//                    ConstantMaterialName.QualityFactor_Aplus => p.priceAplus,
//                    ConstantMaterialName.QualityFactor_A => p.priceA,
//                    ConstantMaterialName.QualityFactor_B => p.priceB,
//                    _ => 0
//                }) * (1 - p.Discount*0.01f),
//                purchasePrice = p.count * (request.QualityFactor switch
//                {
//                    ConstantMaterialName.QualityFactor_A2plus => p.priceA2plus,
//                    ConstantMaterialName.QualityFactor_Aplus => p.priceAplus,
//                    ConstantMaterialName.QualityFactor_A => p.priceA,
//                    ConstantMaterialName.QualityFactor_B => p.priceB,
//                    _ => 0
//                })/2,
//            }).ToList();

//            Console.WriteLine();
//            return new ResultDto<ProductFactorGetResult>()
//            {
//                Data = new ProductFactorGetResult
//                {
//                    ProductFactorsInfo = ProductFactorsList,
//                },
//                IsSuccess = true,
//                Message = " دریافت موفقیت آمیز "

//            };
//        }












//        public ResultDto<ProductFactorComplementationResult> GetProductForComplementation(long  factorId)
//        {
//            var factor = _context.MainFactors.FirstOrDefault(i => i.Id == factorId && i.IsRemoved == false&& i.state==3);
//            if (factor == null)
//            {


//                return new ResultDto<ProductFactorComplementationResult>()
//                {
                  
//                    IsSuccess = false,
//                    Message = " فاکتوری وجود ندارد "

//                };

//            }
//            var subFactor= _context.SubFactors.Where(p=>p.FactorID==factorId&& p.status).ToList();
//            if (subFactor.Count!=1)
//            {


//                return new ResultDto<ProductFactorComplementationResult>()
//                {

//                    IsSuccess = false,
//                    Message = " زیرفاکتور وجود ندارد "

//                };

//            }
//            var subFactorId= subFactor.First().Id;
//            var subfactors = _context.ProductFactors.AsQueryable();
//            var ProductFactors = _context.ProductFactors.Where(f =>  f.SubFactorID == subFactorId && f.IsRemoved == false && !f.IsAccessory&& !f.IsUndefinedProduct && !f.IsService).ToList().OrderByDescending(o => o.UpdateTime);

//            var ProductFactorsList = ProductFactors.Select(p => new ProductFactorComplementationDto
//            {
//                id = p.Id,
//                label = p.Name,
//                Discount = p.Discount,
//                svg=p.ProductDetails.ToString(),
//                InsertTime = p.InsertTime,
//                price = p.count * p.fee * (1 - p.Discount * 0.01f),
                
//            }).ToList();

//            foreach(var item in ProductFactorsList)
//            {
//                dynamic Detail = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(item.svg);

//                string svg = Detail.file;
//                item.svg = svg;
//            }
//            Console.WriteLine();
//            return new ResultDto<ProductFactorComplementationResult>()
//            {
//                Data = new ProductFactorComplementationResult
//                {
//                    ProductFactorsInfo = ProductFactorsList,
//                },
//                IsSuccess = true,
//                Message = " دریافت موفقیت آمیز "

//            };
//        }




//    }



//    public class ProductFactorGetRequest
//    {
//        public long FactorId { get; set; }
//        public long SubFactorID { get; set; }
//        public string QualityFactor { get; set; }
//        public bool IsAccessory { get; set; }
//    }

//    public class ProductFactorGetDto
//    {
//        public long id { get; set; }
//        public string Name { get; set; }
//        public int count { get; set; }
//        public float fee { get; set; }
//        public float purchaseFee { get; set; } = 0;
//        public float Discount { get; set; }
//        public float price { get; set; }
//        public float purchasePrice { get; set; } = 0;

//        public DateTime InsertTime { get; set; }
//    }

//    public class ProductFactorComplementationDto
//    {
//        public long id { get; set; }
//        public string label { get; set; }
        
//        public float Discount { get; set; }
//        public float price { get; set; }
//        public string svg { get; set; }
//        public DateTime InsertTime { get; set; }

//    }

//    public class ProductFactorGetResult
//    {
//        public List<ProductFactorGetDto> ProductFactorsInfo { get; set; }
//    }
//    public class ProductFactorComplementationResult
//    {
//        public List<ProductFactorComplementationDto> ProductFactorsInfo { get; set; }
//    }


//}
