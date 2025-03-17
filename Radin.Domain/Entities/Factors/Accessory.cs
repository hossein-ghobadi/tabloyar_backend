using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class Accessory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float MinimumQuantity { get; set; } = 1;
        public float fee { get; set; }
        public float purchaseFee { get; set; }

        public string? Description { get; set; }
    }
}
