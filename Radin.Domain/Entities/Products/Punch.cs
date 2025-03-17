using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products
{
    public class Punch
    {
        public int Id { get; set; }
        public string PunchTitle { get; set; }
        public string PunchModel { get; set; }
        public float PunchFee { get; set; }
        public string? QualityFactor { get; set; }
        public bool? IsDefault { get; set; } = false;
    }
}
