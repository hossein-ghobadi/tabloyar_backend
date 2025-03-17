using Radin.Application.Interfaces.Contexts;
using Radin.Common;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Queries.ProductPriceDetailGet
{
    public interface IProductPriceDetailGetSevice
    {
        Task<ResultDto<ProductPriceDetailResultGet>> ProductPriceDetails(long productId);

    }
    public class ProductPriceDetailGetSevice : IProductPriceDetailGetSevice
    {

        private readonly IDataBaseContext _context;
        public ProductPriceDetailGetSevice(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<ProductPriceDetailResultGet>> ProductPriceDetails(long productId)
        {

            var Product = _context.ProductFactors.FirstOrDefault(p => p.Id == productId && !p.IsRemoved);
            if (Product == null)
            {
                return new ResultDto<ProductPriceDetailResultGet> { IsSuccess = false, Message = "محصول وجود ندارد" };

            }
            var Factor = _context.MainFactors.Where(p => p.Id == Product.FactorID && !p.IsRemoved).Select(p => new { p.WorkName }).FirstOrDefault();

            var subfactor = _context.SubFactors.FirstOrDefault(p => p.Id == Product.SubFactorID && !p.IsRemoved);
            if (subfactor == null || Factor == null)
            {
                return new ResultDto<ProductPriceDetailResultGet> { IsSuccess = false, Message = "فاکتور وجود ندارد" };

            }
            var QualityFactor = subfactor.QualityFactor;
            float Mfactor = 1;
            if (QualityFactor == ConstantMaterialName.QualityFactor_A2plus)
            {
                Mfactor = 1.2f;
                QualityFactor=ConstantMaterialName.QualityFactor_Aplus;
            }
            var Result = _context.ProductPriceDetails.FirstOrDefault(p => p.ProductId == productId && p.QualityFactor == QualityFactor);
            if (Result == null)
            {
                return new ResultDto<ProductPriceDetailResultGet> { IsSuccess = false, Message = "جزییات محصول وجود ندارد" };

            }
            
            var preparedResult = new List<IdLabelString>
            {
                new(){id="درجه کیفی" ,label=QualityFactor},
                new(){id="هزینه لبه" ,label=((int)(Result.EdgeCost*Mfactor)).ToString()},
                new(){id="هزینه دستمزد لبه" ,label=((int)(Result.EdgeWorkerCost*Mfactor)).ToString()},
                new(){id="هزینه چسب" ,label=((int)(Result.GlueCost*Mfactor)).ToString()},
                new(){id="هزینه متریال لایه بیرونی" ,label=((int)(Result.MLayoutCost*Mfactor)).ToString()},


            };


            if (Result.PvcLayoutCost != 0) { preparedResult.Add(new() { id = "هزینه PVC", label = ((int)(Result.PvcLayoutCost * Mfactor)).ToString() }); }
            if (Result.FSmdCost != 0) { preparedResult.Add(new() { id = "هزینه SMD جلوی کار", label = ((int)(Result.FSmdCost * Mfactor)).ToString() }); }
            if (Result.BSmdCost != 0) { preparedResult.Add(new() { id = "هزینه SMD بک لایت", label = ((int)(Result.BSmdCost * Mfactor)).ToString() }); }
            if (Result.FSmdCount != 0) { preparedResult.Add(new() { id = "تعداد SMD جلوی کار", label = ((int)Result.FSmdCount ).ToString() }); }
            if (Result.BSmdCount != 0) { preparedResult.Add(new() { id = "تعداد SMD بک لایت", label = ((int)Result.BSmdCount ).ToString() }); }
            if (Result.PunchCost != 0) { preparedResult.Add(new() { id = "هزینه پانچ", label = ((int)(Result.PunchCost * Mfactor)).ToString() }); }
            if (Result.CrystalCost != 0) { preparedResult.Add(new() { id = "هزینه کریستال", label = ((int)(Result.CrystalCost * Mfactor)).ToString() }); }
            if (Result.SecondMLayoutCost != 0) { preparedResult.Add(new() { id = "هزینه متریال لایه داخلی", label = ((int)(Result.SecondMLayoutCost * Mfactor)).ToString() }); }
            if (Result.powerCost != 0) { preparedResult.Add(new() { id = "هزینه پاور", label = ((int)(Result.powerCost * Mfactor)).ToString() }); }
            preparedResult.Add(new() { id = "مجموع هزینه ها", label = ((int)(Result.ProcuctCost * Mfactor)).ToString() });


            var saveName = $"جزییات قیمت_{Factor.WorkName}_{Product.Name}_{Product.Id}";
            var PriceDetail = new ProductPriceDetailResultGet { DetailList = preparedResult, SaveName = saveName };
            return new ResultDto<ProductPriceDetailResultGet> { Data = PriceDetail, IsSuccess = true, Message = "دریافت موفق" };
        }
    }





    public class ProductPriceDetailResultGet
    {
        public List<IdLabelString> DetailList { get; set; }
        public string SaveName { get; set; }


    }
}
