using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products
{
    public class Power
    {
        public int Id { get; set; }
        public int PowerType { get; set; }
        public int MaxSmd { get; set; }

        public float PowerFee { get; set; }
        public string QualityFactor { get; set; }
        public bool? IsDefault { get; set; } = false;
    }
}
