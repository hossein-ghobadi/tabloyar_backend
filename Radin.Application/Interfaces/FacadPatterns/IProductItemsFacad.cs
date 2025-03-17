using Radin.Application.Services.ProductItems.Queries.ChannelliumGet;
using Radin.Application.Services.ProductItems.Queries.PlasticGet;
using Radin.Application.Services.ProductItems.Queries.SwediMaxGet;
using Radin.Application.Services.ProductItems.Queries.TitleGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Interfaces.FacadPatterns
{
    public interface IProductItemsFacad
    {
        IChannelliumGet ChannelliumGet { get; }
        IPlasticGetService PlasticGetService { get; }
        ISwediMaxGetService SwediMaxGetService { get; }
        ITitleGetService TitleGetService { get; }
    }
}
