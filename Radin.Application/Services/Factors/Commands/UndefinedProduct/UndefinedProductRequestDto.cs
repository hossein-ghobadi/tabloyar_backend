using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Commands.UndefinedProduct
{
    public class UndefinedProductRequestDto
    {

        public long? factorId { get; set; }
        public string? QualityFactor { get; set; }
        public long? subFactorId { get; set; } = 0;
        public long productId { get; set; } = 0;
        public string? label { get; set; }
        public float fee { get; set; }
        public int? count { get; set; } = 1;
        public float Discount { get; set; } = 0;
        public string Description { get; set; }
        //public string Image { get; set; }
        public List<string>? Image { get; set; } 
        public string Id { get; set; }
    }

    public class DescriptionImage
    {
        public string Description { get; set; }
        public string? Image { get; set; }
        public string Id { get; set; }
    }
}
