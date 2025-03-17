using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Comments
{
    public class SubComment : BaseEntity
    {
        public virtual Comment Comment { get; set; }
        public long CommentID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string ReplyMsg { get; set; }
        public long ContentId { get; set; }

    }
}
