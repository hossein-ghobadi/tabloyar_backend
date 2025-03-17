using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Ideas
{
    public class Idea : BaseEntity
    {
        public long Id { get; set; }
        public string IdeaOwnerId { get; set; }
        public string IdeaOwnerName { get; set; }

        public bool OnlinePrice { get; set; }
        public float AverageStar { get; set; }
        public int SumStar { get; set; }
        public int CountStar { get; set; }

        public string IdeaTitle { get; set; }
        public string IdeaUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int IdeaSorting { get; set; }
        public string IdeaLongDescription { get; set; }
        public string IdeaMetaDesc { get; set; }
        public string IdeaImageAlt { get; set; }
        public bool IdeaPublish { get; set; }
        public string MainImage {  get; set; }
        public string IdeaImage {  get; set; }
        public bool IsIndex { get; set; } = false;

        public virtual ICollection<IdeaRank> IdeaRanks { get; set; }


        public string IdeaCategoryUniqeName { get; set; }
        public string IdeaCategoryTitle { get; set; }
    }
}
