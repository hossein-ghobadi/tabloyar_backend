using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Customers
{
    public class JobCategoryInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? description { get; set; }
        public bool IsDefault { get; set; }=false;
    }
}
