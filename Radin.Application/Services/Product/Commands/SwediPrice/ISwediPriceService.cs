using Radin.Application.Services.Product.Commands.PlasticPrice;
using Radin.Application.Services.Product.Commands.PowerCalculation;
using Radin.Application.Services.Product.Commands.Swedi;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Product.Commands.SwediPrice
{
    public interface ISwediPriceService
    {
        ResultDto<ResultSwediPriceDto, CalculationResult> Execute(RequestSwediPriceDto request1, RequestSwediNfpInfoDto request2, string QualityFactor);
        ResultDto<AllQfSwediResultDto> AllQfCalculation(RequestSwediPriceDto request1, RequestSwediNfpInfoDto request2);

        ResultDto<MultipartFormDataContent> NestingInputsGet(string address, string secondLayer, float margin, int secondLayerState);

    }
}
