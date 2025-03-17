using Microsoft.AspNetCore.Identity;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Ideas
{
    public class IdeaRank
    {
        public int Id { get; set; }
        public int StarPoint {  get; set; }

        public string UserId { get; set; }
        //public virtual IdentityUser User { get; set; }

        public long IdeaId { get; set; }
        public virtual Idea Idea { get; set; }
    }
}
