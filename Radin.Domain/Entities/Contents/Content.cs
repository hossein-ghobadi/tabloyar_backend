using Radin.Domain.Entities.Comments;
using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Contents
{
    public class Content : BaseEntity
    {
        public long Id { get; set; }
        public string ContentTitle { get; set; }
        public string ContentUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int ContentSorting { get; set; }
        public string ContentLongDescription { get; set;}
        public string ContentMetaDesc { get; set; }
        public string ContentImageAlt { get; set; }
        public string? ContentImageTitle { get; set; }

        public bool ContentPublish { get; set; }
        public string ContentImage {  get; set; }
        public string CategoryUniqeName { get; set; }
        public string CategoryTitle { get; set; }
        public string? Canonical {  get; set; }
        public bool IsIndex { get; set; } = false;

    }
}
