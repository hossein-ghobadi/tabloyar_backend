using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Product.Commands.SwediMaxPrice
{
    public class RequestSwediMaxNfpInfoDto
    {
        public float LRealPvc { get; set; }
        public float AConsumptionPvc { get; set; }
        public float AConsumptionM1 { get; set; }
        public float AConsumptionM2 { get; set; }
        public float secondLayerRealArea { get; set; }
        public float BacklightConsumption { get; set; } = 0;

        public float ARealPvc { get; set; }
    }
}
