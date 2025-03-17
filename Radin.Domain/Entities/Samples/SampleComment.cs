using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Samples
{
    public class SampleComment : BaseEntity
    {

        public long Id { get; set; }
        public string SampleCategoryUniqeName { get; set; }
        public string SampleTitle { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string CommentText { get; set; }
        public string Situation { get; set; }
        public long SampleId { get; set; }
    }
}
