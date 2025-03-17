using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products
{
    public class ColorCost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float ColorHardness { get; set; }
        public float ColorFee1 { get; set; }
        public float ColorFee2 { get; set; }
    }
}
