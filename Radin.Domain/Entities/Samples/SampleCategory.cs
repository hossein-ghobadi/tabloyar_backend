using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Samples
{
    public class SampleCategory : BaseEntity
    {
        public long Id { get; set; }
        public string SampleCategoryTitle { get; set; }
        public string SampleCategoryUniqeName { get; set; }
        public int SampleCategorySorting { get; set; }

        //public string? SampleCategoryStyle { get; set; }
        //public bool SampleCategoryIsShowMain { get; set; }
        public bool SampleCategoryIsShowMenu { get; set; }
        public string? SampleCategoryDescription { get; set; }
    }
}
