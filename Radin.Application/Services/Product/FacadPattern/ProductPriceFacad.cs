using Radin.Application.Interfaces.Contexts;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Product.Commands.ChallPrice;
using Radin.Application.Services.Product.Commands.Mapping;
using Radin.Application.Services.Product.Commands.PlasticPrice;
using Radin.Application.Services.Product.Commands.PowerCalculation;
using Radin.Application.Services.Product.Commands.SwediMaxPrice;
using Radin.Application.Services.Product.Commands.SwediPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Product.FacadPattern
{
    public class ProductPriceFacad : IProductPriceFacad
    {
        private readonly IPriceFeeDataBaseContext _context;
        private readonly IPowerCalculationService _powerCalculationService;
        public ProductPriceFacad (
            IPriceFeeDataBaseContext context,
            IPowerCalculationService powerCalculationService
            
            )

        {

            _context = context;
            _powerCalculationService = powerCalculationService;
        
        }

        private ChallMappingDto _challMappingDto;
        public ChallMappingDto ChallMappingDto
        {
            get
            {
                return _challMappingDto = _challMappingDto ?? new ChallMappingDto();
            }
        }


        private PlasticMappingDto _plasticMappingDto;
        public PlasticMappingDto PlasticMappingDto
        {
            get
            {
                return _plasticMappingDto = _plasticMappingDto ?? new PlasticMappingDto();
            }
        }

        private SwediMappingDto _swediMappingDto;
        public SwediMappingDto SwediMappingDto
        {
            get
            {
                return _swediMappingDto = _swediMappingDto ?? new SwediMappingDto();
            }
        }

        private SwediMaxMappingDto _swediMaxMappingDto;
        public SwediMaxMappingDto SwediMaxMappingDto
        {
            get
            {
                return _swediMaxMappingDto = _swediMaxMappingDto ?? new SwediMaxMappingDto();
            }
        }

        private IChallPriceService _challPriceService;
        public IChallPriceService ChallPriceService { 
            get 
            {
                return _challPriceService=_challPriceService ?? new ChallPriceService(_context,_powerCalculationService);
            }
        }

        private IPlasticPriceService _plasticPriceService;
        public IPlasticPriceService PlasticPriceService
        {
            get
            {
                return _plasticPriceService = _plasticPriceService ?? new PlasticPriceService(_context, _powerCalculationService);
            }
        }

        private ISwediPriceService _swediPriceService;
        public ISwediPriceService SwediPriceService
        {
            get
            {
                return _swediPriceService = _swediPriceService ?? new SwediPriceService(_context, _powerCalculationService);
            }
        }


        private ISwediMaxPriceService _swediMaxPriceService;
        public ISwediMaxPriceService SwediMaxPriceService
        {
            get
            {
                return _swediMaxPriceService = _swediMaxPriceService ?? new SwediMaxPriceService(_context, _powerCalculationService);
            }
        }
    }
}
