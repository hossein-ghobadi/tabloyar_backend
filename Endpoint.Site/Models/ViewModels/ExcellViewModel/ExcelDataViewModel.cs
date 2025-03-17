using Radin.Domain.Entities.Products;
using Radin.Domain.Entities.Products.Aditional;

namespace Endpoint.Site.Models.ViewModels.ExcellViewModel
{
    public class ExcelDataViewModel
    {
        public List<Material> Materials { get; set; }
        public List<EdgeProperty> EdgeProperties { get; set; }
        public List<Crystal> Crystals { get; set; }
        public List<ColorCost> ColorCosts { get; set; }
        public List<EdgePunch> EdgePunches { get; set; }
        public List<Glue> Glues { get; set; }
        public List<Power> Powers { get; set; }
        public List<Punch> Punches { get; set; }
        public List<SecondLayerMaterial> SecondLayerMaterials { get; set; }
        public List<Smd> Smds { get; set; }
        public List<Margin> Margins { get; set; }
        public List<MaterialColor> MaterialColors { get; set; }
        public List<MaterialEdgeColor> MaterialEdgeColors { get; set; }
        public List<MaterialEdgeSize> MaterialEdgeSizes { get; set; }
        public List<QualityDegree> QualityDegrees { get; set; }
        public List<Title> Titles { get; set; }
    }

}
