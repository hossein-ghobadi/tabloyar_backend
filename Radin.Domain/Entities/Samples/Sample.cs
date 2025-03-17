using Radin.Domain.Entities.Commons;
using Radin.Domain.Entities.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Samples
{
    public class Sample: BaseEntity
    {
        public long Id { get; set; }
        public string SampleOwnerId { get; set; }
        public string SampleOwnerName { get; set; }

        public bool OnlinePrice { get; set; }
        public float AverageStar { get; set; }
        public int SumStar { get; set; }
        public int CountStar { get; set; }

        public string SampleTitle { get; set; }
        public string SampleUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int SampleSorting { get; set; }
        public string SampleLongDescription { get; set; }
        public string SampleMetaDesc { get; set; }
        public string SampleImageAlt { get; set; }
        public bool SamplePublish { get; set; }
        public string MainImage { get; set; }
        public string SampleImage { get; set; }
        public bool IsIndex { get; set; } = false;

        public virtual ICollection<SampleRank> SampleRanks { get; set; }


        public string SampleCategoryUniqeName { get; set; }
        public string SampleCategoryTitle { get; set; }
    }
}
