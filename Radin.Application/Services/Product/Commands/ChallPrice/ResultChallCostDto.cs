using Radin.Common.Dto;

namespace Radin.Application.Services.Product.Commands.ChallPrice
{

    public partial class ChallPriceService
    {
        public class ResultChallCostDto: ResultWithQualityDegree
        {
            public float ProductCost { get; set; }
            public float edgeCost { get; set; }
            public float edgeWorkerCost { get; set; }
            public float fSmdCost { get; set; }
            public float fSmdCount { get; set; } = 0;
            public float bSmdCost { get; set; }
            public float bSmdCount { get; set; } = 0;
            public float glueCost { get; set; }
            public float punchCost { get; set; }
            public float crystalCost { get; set; }
            public float mLayoutCost { get; set; }
            public float SecondMLayoutCost { get; set; }

            public float pvcLayoutCost { get; set; }
            public float powerCost { get; set; }
            public float lRealPvc { get; set; }
            public float aConsumptionPvc { get; set; }
            public float aConsumptionM1 { get; set; }
            public float aConsumptionM2 { get; set; }
            
            public float aRealPvc { get; set; }
            public string powerList { get; set; } = null;

        }
    }
}
