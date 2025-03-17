using Radin.Domain.Entities.Comments;
using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Contents
{
    public class Category : BaseEntity
    {
        public long Id { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryUniqeName { get; set; }
        public int CategorySorting { get; set; }

        public string? CategoryStyle { get; set; }
        public bool CategoryIsShowMain { get; set; }
        public bool CategoryIsShowMenu { get; set; }
        public string? CategoryDescription { get; set; }
    }
}
