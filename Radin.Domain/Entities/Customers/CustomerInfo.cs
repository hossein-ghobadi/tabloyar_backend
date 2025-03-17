using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Customers
{
    public class CustomerInfo : BaseEntity
    {
        public long Id { get; set; }
        public long? CustomerID { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int? Gender { get; set; }
        public int? JobCategory { get; set; }
        public DateTime? Birtday { get; set; }
        public int? AgeCategory { get; set; }
        public int? CharacterType { get; set; }
        public int? acquaintance { get; set; }
        public int? MarketOriented { get; set; }
        public int? Country { get; set; }
        public int? Province { get; set; }//Province
        public int? city { get; set; }
        public string? phone { get; set; }
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Description { get; set; }
        public virtual CharacterTypeDetails CharacterTypeDetails { get; set; }

    }
}
