using Radin.Application.Services.Product.Commands.ChallPrice;
using Radin.Application.Services.Product.Commands.Mapping;
using Radin.Application.Services.Product.Commands.PlasticPrice;
using Radin.Application.Services.Product.Commands.SwediMaxPrice;
using Radin.Application.Services.Product.Commands.SwediPrice;
using Radin.Application.Services.ProductItems.Queries.PlasticGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Interfaces.FacadPatterns
{
    public interface IProductPriceFacad
    {
        IChallPriceService ChallPriceService { get; }
        IPlasticPriceService PlasticPriceService { get; }
        ISwediPriceService SwediPriceService { get; }
        ISwediMaxPriceService SwediMaxPriceService { get; }
        ChallMappingDto ChallMappingDto { get; }
        PlasticMappingDto PlasticMappingDto { get; }
        SwediMappingDto SwediMappingDto { get; }
        SwediMaxMappingDto SwediMaxMappingDto { get; }


    }
}
