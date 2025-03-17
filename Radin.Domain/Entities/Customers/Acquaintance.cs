using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Customers
{
    public class Acquaintance
    {
        public long Id { get; set; }
        public string type { get; set; } = "3";
        public string? description { get; set; }
    }
}
