using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class ProductPriceDetail
    {
        public int Id { get; set; } 
        public long ProductId { get; set; }
        public float ProcuctCost { get; set; } = 0; 
        public float EdgeCost { get; set; } = 0;
        public float EdgeWorkerCost { get; set; } = 0;
        public float FSmdCount { get; set; } = 0;
        public float FSmdCost { get; set; } = 0;
        public float BSmdCount { get; set; } = 0;
        public float BSmdCost { get; set; } = 0;
        public float GlueCost { get; set; } = 0;
        public float PunchCost { get; set; } = 0;
        public float CrystalCost { get; set; } = 0;
        public float MLayoutCost { get; set; } = 0;
        public float PvcLayoutCost { get; set; } = 0;
        public float SecondMLayoutCost { get; set; } = 0;
        public float powerCost { get; set; } = 0;
        public float lRealPvc { get; set; } = 0;
        public float aConsumptionPvc { get; set; } = 0;
        public float aConsumptionM1 { get; set; } = 0;
        public float aConsumptionM2 { get; set; } = 0;
        public string QualityFactor { get; set; }
        public string? powerList { get; set; }

        public ProductFactor ProductFactors { get; set; }

    }
}
