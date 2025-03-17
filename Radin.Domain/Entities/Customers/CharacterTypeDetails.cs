using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Customers
{
    public class CharacterTypeDetails:BaseEntity
    {
        public long Id { get; set; }
        public long CustomerID { get; set; }
        public float D {  get; set; }
        public float I { get; set; }
        public float S { get; set; }
        public float C { get; set; }
        public virtual CustomerInfo Customer { get; set; } // Navigation Property

    }
}
