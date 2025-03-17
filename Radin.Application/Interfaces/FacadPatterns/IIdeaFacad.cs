using Radin.Application.Services.Ideas.Commands.CommentRemove;
using Radin.Application.Services.Ideas.Commands.CommentSet;
using Radin.Application.Services.Ideas.Commands.IdeaCategoryEdit;
using Radin.Application.Services.Ideas.Commands.IdeaCategoryRemove;
using Radin.Application.Services.Ideas.Commands.IdeaCategorySet;
using Radin.Application.Services.Ideas.Commands.IdeaIndex;
using Radin.Application.Services.Ideas.Commands.IdeaRankSet;
using Radin.Application.Services.Ideas.Commands.IdeaSet;
using Radin.Application.Services.Ideas.Commands.IIdeaEdit;
using Radin.Application.Services.Ideas.Commands.IIdeaRemove;
using Radin.Application.Services.Ideas.Queries.IdeaCategoryGet;
using Radin.Application.Services.Ideas.Queries.IdeaGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Interfaces.FacadPatterns
{
    public interface IIdeaFacad
    {
        IIdeaSetService IdeaSetService { get; }
        IIdeaCategorySetService IdeaCategorySetService { get; }
        IIdeaGetService IdeaGetService { get; }
        IIdeaRemoveService IdeaRemoveService { get; }
        IIdeaIndexService IdeaIndexService { get; }
        IIdeaEditService IdeaEditService { get; }

        IIdeaCategoryGetService IdeaCategoryGetService { get; }
        IIdeaCategoryEditService IdeaCategoryEditService { get; }
        IIdeaCategoryRemoveService IdeaCategoryRemoveService { get; }



        IIdeaCommentSetService IdeaCommentSetService { get; }
        IIdeaSubCommentSetService IdeaSubCommentSetService { get;  }
        IIdeaCommentRemoveService IdeaCommentRemoveService { get; }
        IIdeaSubCommentRemoveService IdeaSubCommentRemoveService { get; }

        IIdeaRatingService IdeaRatingService { get; }

    }
}
