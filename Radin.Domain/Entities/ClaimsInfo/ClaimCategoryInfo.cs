using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.ClaimsInfo
{
    public class ClaimCategoryInfo
    {
        public long Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
