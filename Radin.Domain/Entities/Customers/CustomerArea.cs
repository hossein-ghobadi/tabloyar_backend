using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Customers
{
    public class CustomerArea
    {
        public long Id { get; set; }
        public int ProvinceId { get; set; }//ProvinceId
        public int CityId { get; set; }
        public string AreaName { get; set; }
    }
}
