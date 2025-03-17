using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Others
{
    public class StatusReasons:BaseEntity
    {
        public long Id { get; set; }
        public bool status {  get; set; }
        public string Reason { get; set; }
    }
}
