using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Products.Aditional
{
    public class Title
    {
        public int Id { get; set; }
        public string TitleName { get; set; }
        public bool? IsDefault { get; set; } = false;
    }
}
