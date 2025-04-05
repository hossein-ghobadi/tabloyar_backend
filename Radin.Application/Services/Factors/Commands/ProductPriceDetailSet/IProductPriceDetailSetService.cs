//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Factors.Commands.RecordProduct;
//using Radin.Common.Dto;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Domain.Entities.Factors;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Radin.Application.Services.Product.Commands.PowerCalculation;
//using static Radin.Application.Services.Factors.Commands.ProductPriceDetailSet.ProductPriceDetailSetService;
////using static Radin.Application.Services.Factors.Commands.ProductPriceDetailSet;

//namespace Radin.Application.Services.Factors.Commands.ProductPriceDetailSet
//{
//    public interface IProductPriceDetailSetService
//    {
//        Task<ResultDto> PriceDetailSet(ProductPriceDetailRequest request, long productId, string QualityFactor);
//        Task<ProductPriceDetailRequest> PriceRequestMapp(string request);

//    }

//    public class ProductPriceDetailSetService : IProductPriceDetailSetService
//    {
//        private readonly IDataBaseContext _context;


//        public ProductPriceDetailSetService(IDataBaseContext context)
//        {
//            _context = context;

//        }
//        public async Task<ResultDto> PriceDetailSet(ProductPriceDetailRequest request, long productId, string QualityFactor)
//        {
//            //try
//            //{
            
//                var product =_context.ProductFactors.FirstOrDefault(p=>p.Id== productId);
//            if (product == null) { return new ResultDto { IsSuccess = true, Message = "این محصول در جدول محصولات وجود ندارد" }; }
//                var ProductPriceDetail = _context.ProductPriceDetails.FirstOrDefault(p => p.ProductId == productId && p.QualityFactor == QualityFactor);
            
//                if (ProductPriceDetail == null)
//                {
//                    var priceDetail = new ProductPriceDetail
//                    {
//                        ProductId = productId,
//                        QualityFactor = QualityFactor,
//                        ProcuctCost = request.ProductCost,
//                        EdgeCost = request.edgeCost,
//                        EdgeWorkerCost = request.edgeWorkerCost ,
//                        FSmdCost = request.fSmdCost,
//                        FSmdCount = request.fSmdCount,
//                        BSmdCost = request.bSmdCost ,
//                        BSmdCount = request.bSmdCount,
//                        GlueCost = request.glueCost ,
//                        PunchCost = request.punchCost ,
//                        CrystalCost = request.crystalCost ,
//                        MLayoutCost = request.mLayoutCost,
//                        SecondMLayoutCost = request.SecondMLayoutCost,
//                        PvcLayoutCost = request.pvcLayoutCost,
//                        powerCost = request.powerCost ,
//                        lRealPvc = request.lRealPvc ,
//                        aConsumptionM1 = request.aConsumptionM1 ,
//                        aConsumptionM2 = request.aConsumptionM2 ,
//                        powerList = request.powerList,


//                    };
//                    _context.ProductPriceDetails.Add(priceDetail);
//                    _context.SaveChanges();
//                    return new ResultDto { IsSuccess = true, Message = "ثبت موفق ریز قیمت محصول" };

//                }
//                else
//                {

//                    ProductPriceDetail.ProductId = productId;
//                    ProductPriceDetail.QualityFactor = QualityFactor;
//                    ProductPriceDetail.ProcuctCost = request.ProductCost ;
//                    ProductPriceDetail.EdgeCost = request.edgeCost ;
//                    ProductPriceDetail.EdgeWorkerCost = request.edgeWorkerCost;
//                    ProductPriceDetail.FSmdCost = request.fSmdCost;
//                    ProductPriceDetail.FSmdCount = request.fSmdCount;
//                    ProductPriceDetail.BSmdCost = request.bSmdCost ;
//                    ProductPriceDetail.BSmdCount = request.bSmdCount;
//                    ProductPriceDetail.GlueCost = request.glueCost ;
//                    ProductPriceDetail.PunchCost = request.punchCost ;
//                    ProductPriceDetail.CrystalCost = request.crystalCost;
//                    ProductPriceDetail.MLayoutCost = request.mLayoutCost;
//                    ProductPriceDetail.SecondMLayoutCost = request.SecondMLayoutCost;
//                    ProductPriceDetail.PvcLayoutCost = request.pvcLayoutCost;
//                    ProductPriceDetail.powerCost = request.powerCost;
//                    ProductPriceDetail.lRealPvc = request.lRealPvc;
//                    ProductPriceDetail.aConsumptionM1 = request.aConsumptionM1;
//                    ProductPriceDetail.aConsumptionM2 = request.aConsumptionM2;
//                    ProductPriceDetail.powerList= request.powerList;

//                    _context.SaveChanges();
//                    return new ResultDto { IsSuccess = true, Message = "ویرایش موفق ریز قیمت محصول" };


//                }
//            //}
//            //catch
//            //{
//            //    return new ResultDto { IsSuccess = false, Message = "اشکال درثبت  ریز قیمت محصول" };

//            //}

//        }







//        public async Task<ProductPriceDetailRequest> PriceRequestMapp(string request)
//        {


//            dynamic Detail = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(request);
//            var Result = new ProductPriceDetailRequest();
//            Result.ProductCost = Detail.ProductCost;
//            Result.edgeCost = Detail.edgeCost;
//            Result.edgeWorkerCost = Detail.edgeWorkerCost;
//            Result.fSmdCost = Detail.fSmdCost;
//            Result.fSmdCount = Detail.fSmdCount;
//            Result.bSmdCost = Detail.bSmdCost;
//            Result.bSmdCount = Detail.bSmdCount;
//            Result.glueCost = Detail.glueCost;
//            Result.punchCost = Detail.punchCost;
//            Result.crystalCost = Detail.crystalCost;
//            Result.mLayoutCost = Detail.mLayoutCost;
//            Result.SecondMLayoutCost = Detail.SecondMLayoutCost;
//            Result.pvcLayoutCost = Detail.pvcLayoutCost;
//            Result.powerCost = Detail.powerCost;
//            Result.lRealPvc = Detail.lRealPvc;
//            Result.aConsumptionPvc = Detail.aConsumptionPvc;
//            Result.aConsumptionM1 = Detail.aConsumptionM1;
//            Result.aConsumptionM2 = Detail.aConsumptionM2;
//            Result.aRealPvc = Detail.aRealPvc;
//            Result.powerList= Detail.powerList;
//            return Result;



//        }
//    }
//        public class ProductPriceDetailRequest
//        {
//            public float ProductCost { get; set; } = 0;
//            public float edgeCost { get; set; } = 0;
//            public float edgeWorkerCost { get; set; } = 0;
//            public float fSmdCost { get; set; } = 0;
//            public float fSmdCount { get; set; } = 0;
//            public float bSmdCost { get; set; } = 0;
//            public float bSmdCount { get; set; } = 0;
//            public float glueCost { get; set; } = 0;
//            public float punchCost { get; set; } = 0;
//            public float crystalCost { get; set; } = 0;
//            public float mLayoutCost { get; set; } = 0;
//            public float SecondMLayoutCost { get; set; } = 0;
//            public float pvcLayoutCost { get; set; } = 0;
//            public float powerCost { get; set; } = 0;
//            public float lRealPvc { get; set; } = 0;
//            public float aConsumptionPvc { get; set; } = 0;
//            public float aConsumptionM1 { get; set; } = 0;
//            public float aConsumptionM2 { get; set; } = 0;
//            public float aRealPvc { get; set; } = 0;
//            public string? powerList { get; set; }
//        }
    
//}
