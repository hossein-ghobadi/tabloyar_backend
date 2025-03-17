using Radin.Application.Services.Product.Commands.ChallPrice;
using Radin.Application.Services.Product.Commands.PowerCalculation;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Product.Commands.PlasticPrice
{
    public interface IPlasticPriceService
    {
        ResultDto<ResultPlasticPriceDto, CalculationResult> Execute(RequestPlasticPriceDto request1, RequestPlasticNfpInfoDto request2, string QualityFactor);
        ResultDto<AllQfPlasticResultDto> AllQfCalculation(RequestPlasticPriceDto request1, RequestPlasticNfpInfoDto request2);

        ResultDto<MultipartFormDataContent> NestingInputsGet(string address, string secondLayer, float margin, int secondLayerState);

    }
}
