using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products.Aditional
{
    public class Margin
    {
        public int Id { get; set; }
        public string MarginTitle { get; set; }
        public float MarginNumber { get; set; }
        public bool? IsDefault { get; set; } = false;

    }
}
