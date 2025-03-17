using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products
{
    public class EdgeProperty
    {
        public int Id { get; set; }
        public string EdgeTitle { get; set; }
        public float EdgeSize { get; set; }
        public string? EdgeColor { get; set; }
        public float? EdgeThickness { get; set; }
        public string? EdgeSecondColor { get; set; }
        public string? ImplementationModel { get; set; }
        public string QualityFactor { get; set; }
        public float EdgeWorkerFee { get; set; }
        public float EdgeFee { get; set; }
        public float EdgeHardnessFactor { get; set; }
        public bool? IsDefault { get; set; } = false;

    }
}
