using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class FactorProductComplementary
    {
        public int Id { get; set; }
        public long FactorId { get; set; }
        public long ProductId { get; set; }
        public int ComplementaryId { get; set; }
        public string FirstArg { get; set; }
        public string? SecondArg { get; set; }
        public string Description { get; set; }
    }
}
