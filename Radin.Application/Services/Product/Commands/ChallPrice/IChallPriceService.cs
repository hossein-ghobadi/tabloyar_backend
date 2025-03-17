using Newtonsoft.Json.Linq;
using Radin.Application.Services.Product.Commands.PowerCalculation;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Product.Commands.ChallPrice.ChallPriceService;

namespace Radin.Application.Services.Product.Commands.ChallPrice
{
    public interface IChallPriceService
    {
        ResultDto<ResultChallCostDto, CalculationResult> Execute(RequestChallCostDto request1, RequestNfpInfoDto request2, string QualityFactor);
        ResultDto<AllQfChallResultDto> AllQfCalculation (RequestChallCostDto request1, RequestNfpInfoDto request2);
        ResultDto<MultipartFormDataContent> NestingInputsGet(string address,string secondLayer, float margin, int secondLayerState);
    }
}
