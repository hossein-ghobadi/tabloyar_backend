using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products
{
    public class Smd
    {
        public int Id { get; set; }
        public string SmdTitle { get; set; }

        public string SmdModel { get; set; }
        public string? SmdColor { get; set; }
        public string? SmdSecondColor { get; set; }
        public float SmdFee { get; set; }
        public float SmdWorkerFee { get; set; }
        public float FSmdGoldNumber { get; set; }
        public float BSmdGoldNumber { get; set; }

        public string QualityFactor { get; set; }
        public bool? IsDefault { get; set; } = false;

    }
}
