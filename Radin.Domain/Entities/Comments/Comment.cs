using Radin.Domain.Entities.Commons;
using Radin.Domain.Entities.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Comments
{
    public class Comment : BaseEntity
    {
        public long Id { get; set; }
        public string CategoryUniqeName { get; set; }
        public string ContentTitle { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string CommentText { get; set; }
        public string Situation { get; set; }
        public long ContentId { get; set; }

    }
}
