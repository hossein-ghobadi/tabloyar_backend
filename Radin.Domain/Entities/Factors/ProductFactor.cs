using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class ProductFactor:BaseEntity
    {
        public long Id { get; set; }
        public long SubFactorID { get; set; }
        public long FactorID { get; set;}
        public string Name { get; set; }
        public int count { get; set; } = 1;
        public float fee { get; set; }
        public float price { get; set; }
        public float priceA2plus { get; set; }
        public float priceAplus { get; set; }
        public float priceA { get; set; }
        public float priceB { get; set; }
        public float Discount { get; set; } = 0f;
        public string ProductDetails { get; set; } = "";
        public string NestingResult { get; set; } = "";
        public float PurchaseFee { get; set; } = 0;
        public bool IsAccessory { get; set; }=false;
        public bool IsService { get; set; } = false;
        public int ServiceCode { get; set; } = 0;
        public int AcessoryCode { get; set; } = 0;
        public bool IsUndefinedProduct { get; set; } = false;
        public int UndefinedProductCode { get; set; } = 0;

        public SubFactor SubFactor { get; set; }
        public ICollection<ProductPriceDetail> ProductPriceDetails { get; set; }

    }
}
