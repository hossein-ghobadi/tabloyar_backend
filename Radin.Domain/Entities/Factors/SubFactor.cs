using Radin.Common;
using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Factors
{
    public class SubFactor:BaseEntity
    {
        public long Id { get; set; }
        public long FactorID { get; set; }
        public DateTime RecordTime { get; set; }
        public float Amount { get; set; } = 0;
        public string? Description { get; set; } = "";
        public bool status { get; set; } = false;
        public string QualityFactor { get; set; }="A+";
        public MainFactor MainFactors { get; set; }
        public ICollection<ProductFactor> ProductFactors { get; set; }
    }
}
