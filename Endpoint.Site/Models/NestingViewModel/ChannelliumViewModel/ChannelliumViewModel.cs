using Microsoft.AspNetCore.Mvc;
using Radin.Application.Services.ProductItems.Commands.EdgeSizeEdit;
using Radin.Domain.Entities.Products;

namespace Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel
{
    public class ChannelliumViewModel
    {
        
        public string? QualityFactor { get; set; }
        public long? FactorId { get; set; }  
        public long? SubfactorId { get; set; }
        public long? ProductId { get; set; }
        public string? ProjectName { get; set; }
        public Type boardType { get; set; }
        public data data { get; set; }
        public string file { get; set; } = "";
        public string description { get; set; } = "";
        public List<string>? Images { get; set; }
        //public string ImageString { get; set; } = "";


    }
    public class Type : Base
    {
        public int id { get; set; }
    }
  

    public class data
    {
        public Base? edgeMxecutionModel { get; set; }
        public Base? secondEdgeColor { get; set; }

        public Base? edgesSize { get; set; }
        public needPVC? needPVC { get; set; }
        public Base? edgeColor { get; set; }
        public modelLayerLetters? modelLayerLetters { get; set; }
        public PowerClass? power { get; set; }
        public Punch? isPunch { get; set; }
        public Crystal? needCrystal { get; set; }
        public BackLight? PVCHasBackLight { get; set; }
    }
   
}
