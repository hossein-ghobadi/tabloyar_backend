using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Commands.RecordProduct
{
    public class RecordRequest
    {
        public string? QualityFactor { get; set; }
        public long? factorId {  get; set; }
        public long? productId { get; set; } 
        public long? subFactorId { get; set; }
        public bool priceIsSuccess { get; set; } = true;
        public string priceMessage { get; set; }= "تعیین نشده";
        public QfPrice? productCost { get; set; }
        public string? ProductDetails { get; set; } = "";
        public string description { get; set; } = "";
        public string? productName { get; set; }
        public string NestingResult { get; set; } = "";
        public bool IsAccessory { get; set; }= false;
    }

    public class QfPrice{
        public float Price_A2plus { get; set; }
        public float Price_Aplus {  get; set; }
        public float Price_A { get; set; }
        public float Price_B { get; set; }


    }
    public class UpdateQualityFactorRequest
    {
        public long subFactorId { get; set; }
        public string QualityFactor { get; set; }
    }
 

}
        