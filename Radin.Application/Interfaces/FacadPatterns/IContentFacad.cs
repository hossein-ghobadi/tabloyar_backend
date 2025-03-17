using Radin.Application.Services.Contents.Commands.CommentRemove;
using Radin.Application.Services.Contents.Commands.CommentSet;
using Radin.Application.Services.Contents.Commands.ContentCategoryEdit;
using Radin.Application.Services.Contents.Commands.ContentCategoryRemove;
using Radin.Application.Services.Contents.Commands.ContentCategorySet;
using Radin.Application.Services.Contents.Commands.ContentChangeIsIndex;
using Radin.Application.Services.Contents.Commands.ContentEdit;
using Radin.Application.Services.Contents.Commands.ContentRemove;
using Radin.Application.Services.Contents.Commands.ContentSet;
using Radin.Application.Services.Contents.Commands.SubCommentSet;
using Radin.Application.Services.Contents.Queries.CategoryGet;
using Radin.Application.Services.Contents.Queries.CommentInfoGet;
using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
using Radin.Application.Services.Contents.Queries.ContentGet;
using Radin.Application.Services.Contents.Queries.HomeContentGet;
using Radin.Application.Services.Contents.Queries.HomePageContentGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Interfaces.FacadPatterns
{
    public interface IContentFacad
    {


        IContentCategorySetService ContentCategorySetService { get; }
        IContentCategoryGetService ContentCategoryGetService { get; }
        IContentCategoryRemoveService ContentCategoryRemoveService { get; }
        IContentCategoryEditService ContentCategoryEditService { get; }
        IContentSetService ContentSetService { get; }
        IContentGetService ContentGetService { get; }
        IContentRemoveService ContentRemoveService { get; }
        IContentIndexService ContentIndexService { get; }
        IContentEditService ContentEditService { get; }
        IContentTotalGetService ContentTotalGetService { get; }
        IContentCategoryGetSummary ContentCategoryGetSummary { get; }
        ICategoryGetForContentService CategoryGetForContentService { get; }
        IHomeContentGetService HomeContentGetService { get; }
        IHomeUniqeContentGetService HomeUniqeContentGetService { get; }
        IHomeGroupContentGetService HomeGroupContentGetService { get; }
        IHomePageContentGetService HomePageContentGetService { get; }

        ICommentSetService CommentSetService { get; }
        ICommentRemoveService CommentRemoveService { get; }
        ISubCommentRemoveService SubCommentRemoveService { get; }
        ICommentInfoGetService CommentInfoGetService { get; }
        ISubCommentSetService SubCommentSetService { get; }
    }
}
