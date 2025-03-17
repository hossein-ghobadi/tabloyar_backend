using Radin.Domain.Entities.Samples;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Samples
{
    public class SampleRank
    {

        public int Id { get; set; }
        public int StarPoint { get; set; }

        public string UserId { get; set; }
        //public virtual User User { get; set; }

        public long SampleId { get; set; }
        public virtual Sample Sample { get; set; }
    }
}
