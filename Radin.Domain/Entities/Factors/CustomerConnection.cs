using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class CustomerConnection:BaseEntity
    {
        public long Id { get; set; }
        public long FactorID { get; set; }
        public DateTime ConnectinTime { get; set; }
        public int ConnectionDuration { get; set; } = 0;
        public int ContactType { get; set; } = 0;
        public string ContactTypeName { get; set; } = "";

        public MainFactor MainFactors { get; set; }

    }
}
