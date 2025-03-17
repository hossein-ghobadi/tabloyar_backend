using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Ideas
{
    public class IdeaComment : BaseEntity
    {
        public long Id { get; set; }
        public string IdeaCategoryUniqeName { get; set; }
        public string IdeaTitle { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string CommentText { get; set; }
        public string Situation { get; set; }
        public long IdeaId { get; set; }
    
    }
}