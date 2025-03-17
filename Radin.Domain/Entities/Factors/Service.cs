using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class Service
    {

        public int Id { get; set; }
        public string ServiceName { get; set; }
        public bool IsDefault { get; set; } = false;

    }
}
