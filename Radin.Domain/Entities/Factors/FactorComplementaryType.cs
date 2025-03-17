using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class FactorComplementaryType
    {
        public int Id { get; set; }
        public int ComplementaryId { get; set; }
        public string Description { get; set; }
    }
}
