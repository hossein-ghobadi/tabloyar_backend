using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products
{
    public class Material
    {

        public int Id { get; set; }
        public string MaterialName { get; set; }

        public string QualityFactor { get; set; }
        public string? MaterialColor { get; set; }
        public float? MaterialThickness { get; set; }
        public float MaterialSizeX { get; set; }
        public float MaterialSizeY { get; set; }
        public float MaterialFee { get; set; }

    }
}
