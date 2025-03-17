using Microsoft.VisualBasic;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentSet;
using Radin.Application.Services.Contents.Queries.CategoryGet;
using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Queries.ContentGet
{
    public interface IContentGetService
    {
        GetContentDto Execute(RequestContentGetDto request);
    }
    public class ContentGetService : IContentGetService
    {
        private readonly IDataBaseContext _context;
        public ContentGetService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public GetContentDto Execute(RequestContentGetDto request)
        {
            //var contents = _context.Contents.AsQueryable();

            var contents = _context.Contents.FirstOrDefault(c => c.ContentUniqeName == request.uniqename);
            if(contents == null)
            {
                return new GetContentDto
                {
                    ContentTitle = "",
                    ContentUniqeName = "",
                    CommentSituation = false,
                    CommentShow = false,
                    ContentSorting = 0,
                    ContentLongDescription = "",
                    ContentMetaDesc = "",
                    ContentImageAlt = "",
                    ContentPublish = false,
                    ContentImage = "",
                    Category = new GetDto
                    {
                        id = "",
                        label = ""

                    },
                    Canonical="",
                    Id = 0,
                    IsRemoved = false,
                    IsIndex = false,
                };
            }

            var contentsList = new GetContentDto
            {
                ContentTitle = contents.ContentTitle,
                ContentUniqeName = contents.ContentUniqeName,
                CommentSituation = contents.CommentSituation,
                CommentShow = contents.CommentShow,
                ContentSorting = contents.ContentSorting,
                ContentLongDescription = contents.ContentLongDescription,
                ContentMetaDesc = contents.ContentMetaDesc,
                ContentImageAlt = contents.ContentImageAlt,
                ContentPublish = contents.ContentPublish,
                ContentImage = contents.ContentImage,
                Category=new GetDto { 
                    id = contents.CategoryUniqeName,
                    label=contents.CategoryTitle
                
                },
                Canonical=contents.Canonical,
                Id = contents.Id,
                IsRemoved =contents.IsRemoved,
                IsIndex =contents.IsIndex,
                ContentImageTitle=contents.ContentImageTitle
            };
            return contentsList;

        }


    }

    public class RequestContentGetDto
    {
        public string uniqename { get; set; }
    }
    public class GetDto
    {
        public string id { get; set; }
        public string label { get; set; }
    }
    public class GetContentDto
    {
        public long Id { get; set; }
        public string ContentTitle { get; set; }
        public string ContentUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int ContentSorting { get; set; }
        public string ContentLongDescription { get; set; }
        public string ContentMetaDesc { get; set; }
        public string ContentImageAlt { get; set; }
        public string? ContentImageTitle { get; set; }

        public bool ContentPublish { get; set; }
        public string ContentImage { get; set; }

        public GetDto Category { get; set; }
        public string Canonical { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsIndex { get; set; }
    }
}
