using Radin.Application.Interfaces.Contexts;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Product.Commands.Mapping;
using Radin.Application.Services.Product.Commands.PowerCalculation;
using Radin.Application.Services.ProductItems.Queries.ChannelliumGet;
using Radin.Application.Services.ProductItems.Queries.PlasticGet;
using Radin.Application.Services.ProductItems.Queries.SwediMaxGet;
using Radin.Application.Services.ProductItems.Queries.TitleGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.ProductItems.FacadPattern
{
    public class ProductItemsFacad : IProductItemsFacad
    {
        private readonly IPriceFeeDataBaseContext _context;
        public ProductItemsFacad(
            IPriceFeeDataBaseContext context

            )

        {
            _context = context;

        }

        private IChannelliumGet _channelliumGet;
        public IChannelliumGet ChannelliumGet
        {
            get
            {
                return _channelliumGet = _channelliumGet ?? new ChannelliumGet(_context);
            }
        }


        private IPlasticGetService _plasticGetService;
        public IPlasticGetService PlasticGetService
        {
            get
            {
                return _plasticGetService = _plasticGetService ?? new PlasticGetService(_context);
            }
        }



        private ISwediMaxGetService _swediMaxGetService;
        public ISwediMaxGetService SwediMaxGetService
        {
            get
            {
                return _swediMaxGetService = _swediMaxGetService ?? new SwediMaxGetService(_context);
            }
        }



        private ITitleGetService _titleGetService;
        public ITitleGetService TitleGetService
        {
            get
            {
                return _titleGetService = _titleGetService ?? new TitleGetService(_context);
            }
        }

    }
}
