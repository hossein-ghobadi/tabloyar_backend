using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Ideas
{
    public class IdeaCategory : BaseEntity
    {
        public long Id { get; set; }
        public string IdeaCategoryTitle { get; set; }
        public string IdeaCategoryUniqeName { get; set; }
        public int IdeaCategorySorting { get; set; }

        //public string? IdeaCategoryStyle { get; set; }
        //public bool IdeaCategoryIsShowMain { get; set; }
        public bool IdeaCategoryIsShowMenu { get; set; }
        public string? IdeaCategoryDescription { get; set; }
    }
}
