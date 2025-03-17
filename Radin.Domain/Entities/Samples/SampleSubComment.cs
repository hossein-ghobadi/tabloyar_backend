using Radin.Domain.Entities.Commons;
using Radin.Domain.Entities.Ideas;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Samples
{
    public class SampleSubComment: BaseEntity
    {

        public virtual SampleComment Comment { get; set; }
        public long CommentID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string ReplyMsg { get; set; }
        public long SampleId { get; set; }
    }
}
