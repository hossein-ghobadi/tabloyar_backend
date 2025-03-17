using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Product.Commands.ChallPrice.ChallPriceService;

namespace Radin.Application.Services.Product.Commands.ChallPrice
{
    public class AllQfChallResultDto
    {

        public ResultChallCostDto Result_Aplus { get; set; }
        public ResultChallCostDto Result_A { get; set; }
        public ResultChallCostDto Result_B { get; set; }
    }
}
