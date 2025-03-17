using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products
{
    public class Crystal
    {
        public int Id { get; set; }
        public string CrystalModel { get; set; }
        public string? CrystalColor { get; set; }

        public float CrystalFee { get; set; }
        public string QualityFactor { get; set; }
        public bool? IsDefault { get; set; } = false;
    }
}
