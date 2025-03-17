using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Product.Commands.RecordProduct
{
    public class RecordRequest
    {
        public long? factorId {  get; set; }
        public long? productId { get; set; } 
        public long? subFactorId { get; set; }
        public bool priceIsSuccess { get; set; }
        public string priceMessage { get; set; }
        public float productCost { get; set; }
        public string? ProductDetails { get; set; }
        public string description { get; set; }
        public string productName { get; set; }
    }
}
        