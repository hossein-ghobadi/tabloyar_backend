using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Claim
{
    public class ClaimInfo
    {
        public long Id { get; set; }
        public long? category { get; set; }
        public string ClaimName1 {  get; set; }
        public string ClaimName2 { get; set; }

    }
}
