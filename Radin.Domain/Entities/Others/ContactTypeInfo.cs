using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Others
{
    public class ContactTypeInfo
    {
        public long Id { get; set; }
        public string type { get; set; }
        public string? description { get; set; }
    }
}
