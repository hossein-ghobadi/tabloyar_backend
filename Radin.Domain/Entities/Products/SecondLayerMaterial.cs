using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products
{
    public class SecondLayerMaterial
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string MaterialName { get; set; }

        public bool? IsDefault { get; set; }

    }
}
