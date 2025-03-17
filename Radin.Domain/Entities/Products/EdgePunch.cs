using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products
{
    public class EdgePunch
    {
        public int Id { get; set; }
        public string EdgePunchTitle { get; set; }
        public string EdgePunchModel { get; set; }
        public float EdgePunchFee { get; set; }
        public string QualityFactor { get; set; }
        public bool? IsDefault { get; set; } = false;
    }
}
