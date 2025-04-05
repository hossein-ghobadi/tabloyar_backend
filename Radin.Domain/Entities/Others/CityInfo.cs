using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Others
{
    public class CityInfo
    {
        public int Id { get; set; }
        public string city { get; set; }//ProvinceId
        public string? point { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public int ProvinceId { get; set; }
        public string province { get; set; }
        public string? county { get; set; }
        public string? district { get; set; }
        public string? polygon { get; set; }
    }
}
